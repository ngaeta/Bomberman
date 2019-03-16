using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class LoseState : State
    {
        private float timeToNextScene;

        public override void Enter()
        {
            base.Enter();
            timeToNextScene = 5f;
        }

        public override void Update()
        {
            base.Update();

            PhysicsManager.Update();
            UpdateManager.Update();

            if (timeToNextScene <= 0)
            {
                Game.CurrScene.IsPlaying = false;
            }
            else
                timeToNextScene -= Game.DeltaTime;
        }
    }
}
