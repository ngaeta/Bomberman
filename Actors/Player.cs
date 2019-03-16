using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using Aiv.Fast2D;
using Aiv.Audio;

namespace Bomberman
{
    public enum PlayerNumber { One, Two, Three, Four }

    abstract class Player : GameObject, IHittable
    {
        const int SCORE_ON_HITTED = 800;
        const float TIME_INVULNERABILITY = 4f;

        private const float RANGE_BOMB = 0.5f;
        private AnimationType currAnim;
        private bool isHitted;
        private List<PowerUp> activePowerUps;
        private float timeBlink;
        private float currTimeInvulnerability;
        private float timeToDisappear;
        private AudioSource audioSource;
        private AudioClip clipOnHitted;

        protected float timeToNextBomb;
        protected enum AnimationType { WalkDown, WalkUp, WalkLeft, Win, Death, WalkRight }

        public bool HavingExtraLife { get; set; }
        public bool IsDead { get; protected set; }
        public float Speed { get; protected set; }
        public bool IsInvincible { get; private set; }

        public int Score { get; protected set; }
        public Vector2 GUIPosition { get; protected set; }
        public string GUIImage { get; protected set; }

        public Player(Vector2 spritePosition, string spriteName) : base(spritePosition, spriteName, DrawManager.Layer.Foreground)
        {
            Speed = 4;
            sprite.scale /= 1.7f;
            activePowerUps = new List<PowerUp>();

            Rect rect = new Rect(new Vector2(0.25f, 0.25f), null, Width * 0.8f, Height);
            RigidBody = new RigidBody(spritePosition, this, null, rect, false);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Player;
            RigidBody.SetCollisionMask((uint)(PhysicsManager.ColliderType.Obstacle | PhysicsManager.ColliderType.Pickable));

            currAnim = AnimationType.WalkDown;
            animations[(int)AnimationType.Death].LoopAtFrame = 2;
            Animation.Pause();

            Score = 0;
            timeToDisappear = 2.5f;

            clipOnHitted = AudioManager.GetAudioClip("death");
        }

        public void Input()
        {
            if (!IsDead && currAnim != AnimationType.Win)
            {
                RigidBody.Velocity = Vector2.Zero;
                OnInput();
            }
        }

        public override void Update()
        {
            base.Update();

            if (!IsDead)
            {
                if (timeToNextBomb > 0)
                {
                    timeToNextBomb -= Game.DeltaTime;
                }

                if (IsInvincible)
                {
                    timeBlink += Game.DeltaTime * 20;
                    float multiply = (float)Math.Cos(timeBlink);
                    sprite.SetMultiplyTint(new Vector4(0.8f, multiply, multiply, 1));
                }
                else 
                {
                    if (currTimeInvulnerability <= 0)
                    {
                        sprite.SetMultiplyTint(Vector4.One);
                        
                    }
                    else
                    {
                        timeBlink += Game.DeltaTime * 30;
                        float multiply = (float)Math.Cos(timeBlink);
                        sprite.SetMultiplyTint(new Vector4(multiply, multiply, multiply, multiply));
                        currTimeInvulnerability -= Game.DeltaTime;
                    }
                }
            }
            else if(IsActive)
            {
                if (timeToDisappear <= 0)
                {
                    new DisappearPlayerEffect(Position);
                    IsActive = false;
                }
                else 
                    timeToDisappear -= Game.DeltaTime;
            }
        }

        public override void OnCollide(Collision collision)
        {
            base.OnCollide(collision);

            if (collision.collider is Obstacle)
            {
                float deltaX = collision.Delta.X;
                float deltaY = collision.Delta.Y;

                if (deltaX < deltaY)
                {
                    if (Velocity.X != 0)
                    {
                        //collision from right or left
                        if (Position.X < collision.collider.X)
                        {
                            //from letf
                            deltaX = -deltaX;
                        }

                        Position = new Vector2(Position.X + deltaX, Position.Y);
                        Velocity = new Vector2(0, Velocity.Y);
                    }
                }
                else
                {
                    //collision from top or bottom
                    if (Position.Y < collision.collider.Y)
                    {
                        //from top
                        deltaY = -deltaY;
                    }

                    Position = new Vector2(Position.X, Position.Y + deltaY);
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }
            else if (collision.collider is IPickable pickable)
            {
                if (pickable is PowerUp p)
                {
                    if (!IsAlreadyActive(p))
                    {
                        pickable.OnPlayerPick(this);
                        activePowerUps.Add(p);
                        GuiManager.SwitchGUIPowerUp(this, p, true);
                    }
                }
                else
                {
                    pickable.OnPlayerPick(this);
                }
            }
        }

        public void AddScore(int score)
        {
            Score += score;
            GuiManager.UpdateScores(this);
        }

        public void AddSpeed(float increment)
        {
            Speed += increment;
            Console.WriteLine("Curr vel " + Speed);
        }

        public void RemovePowerUp(PowerUp p)
        {
            if (activePowerUps.Contains(p))
            {
                activePowerUps.Remove(p);
                GuiManager.SwitchGUIPowerUp(this, p, false);
            }
        }

        public void SetInvincibility(bool value = true)
        {
            if (!value)
            {
                timeBlink = 0;
                sprite.SetMultiplyTint(Vector4.One);
            }

            IsInvincible = value;
        }

        protected void ChangeAnimation(AnimationType anim)
        {
            currAnim = anim;

            Animation = animations[(int)anim];
            Animation.Play();
        }

        protected void StopAnimation()
        {
            Animation.Reset();
            Animation.Pause();
        }

        protected void ShootBomb()
        {
            Bomb bomb = BombsManager.GetBomb(this);

            if (bomb != null)
            {
                timeToNextBomb = RANGE_BOMB;
                bomb.Shoot(Position);
            }
        }

        protected abstract void OnInput();

        int IHittable.OnHit(GameObject hitter)
        {
            if (IsDead || IsInvincible || currTimeInvulnerability > 0)
            {
                return 0;
            }

            PlayAudio(clipOnHitted);

            if (HavingExtraLife)
            {
                HavingExtraLife = false;
                currTimeInvulnerability = TIME_INVULNERABILITY;

                for(int i=0; i < activePowerUps.Count; i++)
                {
                    if(activePowerUps[i].Type == PowerUpType.ExtraLife)
                    {
                        RemovePowerUp(activePowerUps[i]);
                    }
                }
                return 0;
            }

            OnDie();
            return SCORE_ON_HITTED;
        }

        bool IHittable.IsHitted()
        {
            return isHitted;
        }

        public void Win()
        {
            Velocity = Vector2.Zero;
            RigidBody.IsCollisionsAffected = false;

            ChangeAnimation(AnimationType.Win);
        }

        private void OnDie()
        {
            IsDead = true;
            isHitted = true;
            Velocity = Vector2.Zero;
            RigidBody.IsCollisionsAffected = false;

            Animation = animations[(int)AnimationType.Death];
            Animation.Play();

            SetInvincibility(false);      
        }

        private void PlayAudio(AudioClip clip)
        {
            audioSource = new AudioSource();
            audioSource.Play(clip);
            AudioManager.DisposeAudioSource(audioSource);
        }

        private bool IsAlreadyActive(PowerUp p)
        {
            for (int i = 0; i < activePowerUps.Count; i++)
            {
                if (activePowerUps[i].Type == p.Type)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
