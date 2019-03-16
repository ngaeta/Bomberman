using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;
using Aiv.Audio;

namespace Bomberman
{
    public enum PowerUpType {SpeedUp, SpeedDown, BombUp, BombDown, Invincibility, ExtraLife}

    abstract class PowerUp : GameObject, IHittable, IPickable
    {
        private bool isHitted;
        private Player playerPicked;
        private AudioSource audioSource;

        protected AudioClip clipOnPicked;
        protected float timeToDeactivePowerUp;

        public PowerUpType Type { get; protected set; }

        public PowerUp(Vector2 spritePosition, string spriteSheetName) : base(spritePosition, spriteSheetName, DrawManager.Layer.Middleground)
        {
            Rect rect = new Rect(new Vector2(sprite.Width / 2, sprite.Height / 2), null, Width, Height);
            RigidBody = new RigidBody(spritePosition, this, null, rect);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Pickable;

            isHitted = false;
            timeToDeactivePowerUp = 3f;

            audioSource = new AudioSource();
            clipOnPicked = AudioManager.GetAudioClip("powerUp1");
        }

        public override void Update()
        {
            base.Update();

            if (playerPicked != null)
            {
                if (timeToDeactivePowerUp <= 0)
                {
                    OnPowerUpDeactive(playerPicked);
                }
                else
                {
                    timeToDeactivePowerUp -= Game.DeltaTime;
                }
            }
        }

        bool IHittable.IsHitted()
        {
            return isHitted;
        }

        int IHittable.OnHit(GameObject hitter)
        {
            isHitted = true;
            IsActive = false;
            new FireBurn(Position).Play();

            return 0;
        }

        void IPickable.OnPlayerPick(Player p)
        {
            audioSource.Play(clipOnPicked);
            OnPlayerPick(p);
        }

        protected virtual void OnPlayerPick(Player p)
        {
            playerPicked = p;
            IsActive = false;
        }

        protected virtual void OnPowerUpDeactive(Player p)
        {
            playerPicked.RemovePowerUp(this);
            playerPicked = null;
            Destroy();
        }
    }
}
