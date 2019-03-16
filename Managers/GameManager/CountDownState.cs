using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace Bomberman
{
    class CountDownState : State
    {
        private float timeCountdown;
        private GuiNumber guiNumber;

        private AudioSource audioSource;
        private AudioClip clipCountDown;

        public override void Enter()
        {
            base.Enter();

            timeCountdown = 3.4f;
            guiNumber = new GuiNumber(new Vector2(Game.Window.OrthoWidth / 2, Game.Window.OrthoHeight / 2), Convert.ToInt16(timeCountdown).ToString());
            guiNumber.Scale *= 2;

            audioSource = new AudioSource();
            clipCountDown = AudioManager.GetAudioClip("countdown");
            audioSource.Play(clipCountDown, true);
        }

        public override void Update()
        {
            base.Update();

            if (timeCountdown <= 0.6f)
            {
                audioSource.Stop();
                machine.Switch((int)GameManager.GameState.Play);
            }
            else
            {
                guiNumber.SetNumber(Convert.ToInt16(timeCountdown).ToString());
                timeCountdown -= Game.DeltaTime;
            }
        }

        public override void Exit()
        {
            base.Exit();
            guiNumber.IsActive = false;
        }
    }
}
