using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Bomberman
{
    class WhiteBomberman : Player
    {
        public WhiteBomberman(Vector2 spritePosition) : base(spritePosition, "whiteBomberman")
        {
            GUIPosition = new Vector2(4, 6);
            GUIImage = "whiteBombermanGUI";
        }

        protected override void OnInput()
        {
            if (Game.Window.GetKey(KeyCode.D))
            {
                RigidBody.SetXVelocity(Speed);
                ChangeAnimation(AnimationType.WalkRight);
            }
            else if (Game.Window.GetKey(KeyCode.A))
            {
                RigidBody.SetXVelocity(-Speed);
                ChangeAnimation(AnimationType.WalkLeft);
            }
            else if (Game.Window.GetKey(KeyCode.W))
            {
                RigidBody.SetYVelocity(-Speed);
                ChangeAnimation(AnimationType.WalkUp);
            }
            else if (Game.Window.GetKey(KeyCode.S))
            {
                RigidBody.SetYVelocity(Speed);
                ChangeAnimation(AnimationType.WalkDown);
            }
            else if (Animation.IsPlaying && Velocity.Length == 0)
            {
                StopAnimation();
            }

            if (Game.Window.GetKey(KeyCode.Space) && timeToNextBomb <= 0)
            {
                ShootBomb();
            }
        }
    }
}
