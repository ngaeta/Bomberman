using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace Bomberman
{
    class DisappearEnemyEffect : GameObject
    {
        private float endTime;
        private AudioSource audioSource;

        public DisappearEnemyEffect(Vector2 spritePosition, float endTime = 0.3f) : base(spritePosition, "disappearEffect", DrawManager.Layer.Foreground)
        {
            this.endTime = endTime;

            audioSource = new AudioSource();
            audioSource.Play(AudioManager.GetAudioClip("disappear1"));
            AudioManager.DisposeAudioSource(audioSource);
        }

        public override void Update()
        {
            base.Update();

            if (endTime <= 0)
            {
                IsActive = false;
                Destroy();
            }
            else
                endTime -= Game.DeltaTime;
        }
    }
}
