using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace Bomberman
{
    class DisappearPlayerEffect : GameObject
    {
        private AudioSource audioSource;

        public DisappearPlayerEffect(Vector2 spritePosition) : base(spritePosition, "disappearBlue", DrawManager.Layer.Foreground)
        {
            sprite.scale = new Vector2(0.9f);

            audioSource = new AudioSource();
            audioSource.Play(AudioManager.GetAudioClip("disappear1"));
            AudioManager.DisposeAudioSource(audioSource);
        }

        public override void Update()
        {
            base.Update();

            if(!Animation.IsPlaying)
            {
                IsActive = false;
                Destroy();
            }
        }
    }
}
