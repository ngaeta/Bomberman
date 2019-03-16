using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;
using OpenTK;

namespace Bomberman
{
    class WinState : State 
    {
        const int SPEED_SHAKE_MULTIPLIER = 2;
        const float WIDTH_SHAKE = 0.3f;

        private GUIItem labelWin;
        private float shakeCounter;

        public override void Enter()
        {
            base.Enter();

            AudioManager.StopBackground();

            for(int i=0; i < PlayScene.Players.Count; i++)
            {
                PlayScene.Players[i].Win();
            }

            AudioManager.SetBackgroundAudio("winner");
            labelWin = new GUIItem(new Vector2(Game.Window.OrthoWidth / 2, Game.Window.OrthoHeight / 2), "win");
        }

        public override void Update()
        {
            base.Update();

            float shake = (float)Math.Sin(shakeCounter) * WIDTH_SHAKE;
            labelWin.GetSprite().scale += new Vector2(shake) * Game.DeltaTime;
            shakeCounter += Game.DeltaTime * SPEED_SHAKE_MULTIPLIER;

            PhysicsManager.Update();
            UpdateManager.Update();
            BombsManager.Update();
        }
    }
}
