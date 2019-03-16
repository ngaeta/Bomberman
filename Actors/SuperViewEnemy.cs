using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class SuperViewEnemy : Enemy
    {
        private enum AnimationType { Down, Right, Up, Left, Die }
        private AnimationType currAnim;

        public SuperViewEnemy(Vector2 spritePosition) : base(spritePosition, "redEnemy")
        {
            currAnim = AnimationType.Down;

            agent.Speed = 2.5f;
            ignoreMaskRaySight.Add(PhysicsManager.ColliderType.Obstacle);  //sees trough the walls
            scoreOnHitted = 400;
        }

        protected override void OnLeftWalking()
        {
            base.OnLeftWalking();
            ChangeAnim(AnimationType.Left);
        }

        protected override void OnRightWalking()
        {
            base.OnRightWalking();
            ChangeAnim(AnimationType.Right);
        }

        protected override void OnUpWalking()
        {
            base.OnUpWalking();
            ChangeAnim(AnimationType.Up);
        }

        protected override void OnDownWalking()
        {
            base.OnDownWalking();
            ChangeAnim(AnimationType.Down);
        }

        private bool ChangeAnim(AnimationType anim)
        {
            if (currAnim != anim)
            {
                Animation = animations[(int)anim];
                Animation.Play();

                currAnim = anim;
                return true;
            }

            return false;
        }
    }
}
