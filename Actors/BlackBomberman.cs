using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Bomberman
{
    class BlackBomberman : Player
    {
        public BlackBomberman(Vector2 spritePosition) : base(spritePosition, "blackBomberman")
        {
            GUIPosition = new Vector2(Game.Window.OrthoWidth - 6, 6);
            GUIImage = "blackBombermanGUI";
        }

        protected override void OnInput()
        {
            if (Game.Window.GetKey(KeyCode.Right))
            {
                RigidBody.SetXVelocity(Speed);
                ChangeAnimation(AnimationType.WalkRight);
            }
            else if (Game.Window.GetKey(KeyCode.Left))
            {
                RigidBody.SetXVelocity(-Speed);
                ChangeAnimation(AnimationType.WalkLeft);
            }
            else if (Game.Window.GetKey(KeyCode.Up))
            {
                RigidBody.SetYVelocity(-Speed);
                ChangeAnimation(AnimationType.WalkUp);
            }
            else if (Game.Window.GetKey(KeyCode.Down))
            {
                RigidBody.SetYVelocity(Speed);
                ChangeAnimation(AnimationType.WalkDown);
            }
            else if (Animation.IsPlaying && Velocity.Length == 0)
            {
                StopAnimation();
            }

            if (Game.Window.GetKey(KeyCode.Return) && timeToNextBomb <= 0)
            {
                ShootBomb();
            }
        }
    }
}
