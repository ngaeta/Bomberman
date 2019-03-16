using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace Bomberman
{
    class Bomb : GameObject
    {
        const float WIDTH_SHAKE = 0.45f;
        const int SPEED_SHAKE_MULTIPLIER = 4;
        const float COUNTDOWN = 3f;

        private float shakeCounter;
        private float currCountdown;
        private AudioSource audioSource;
        private static AudioClip clipExplosion;
        private static AudioClip clipPut;

        public Player PlayerOwner { get; private set; }

        public Bomb(Vector2 spritePosition, Player owner) : base(spritePosition, "bomb", DrawManager.Layer.Middleground)
        {
            IsActive = false;
            PlayerOwner = owner;

            if (clipExplosion == null)
                clipExplosion = AudioManager.GetAudioClip("bombExplosion");

            if (clipPut == null)
                clipPut = AudioManager.GetAudioClip("put");
        }

        public override void Update()
        {
            base.Update();

            if(IsActive)
            {
                if (currCountdown < COUNTDOWN)
                {
                    currCountdown += Game.DeltaTime;

                    float shake = (float)Math.Sin(shakeCounter) * WIDTH_SHAKE;
                    sprite.scale += new Vector2(shake) * Game.DeltaTime;      
                    shakeCounter += Game.DeltaTime * SPEED_SHAKE_MULTIPLIER;

                    sprite.SetAdditiveTint(currCountdown/COUNTDOWN, 0, 0, 0);  //color always more red
                }
                else
                {
                    Explode();
                    RestoreBombInManager();
                }
            }
        }

        public void Shoot(Vector2 position)
        {
            currCountdown = 0;
            Position = position;
            IsActive = true;

            PlayAudio(clipPut);
        }

        private void Explode()
        {
            IsActive = false;
            PlayAudio(clipExplosion);
            new Explosion(Position, this);
        }

        private void RestoreBombInManager()
        {
            BombsManager.EnqueBomb(this);
        }

        private void PlayAudio(AudioClip clip)
        {
            audioSource = new AudioSource();
            audioSource.Play(clip);

            AudioManager.DisposeAudioSource(audioSource);
        }
    }
}
