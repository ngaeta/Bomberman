using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class FastEnemy : Enemy
    {
        private enum AnimationType {UP_DOWN, RIGHT, LEFT, DIE }
        private AnimationType currAnim;

        public FastEnemy(Vector2 spritePosition) : base(spritePosition, "greenEnemy")
        {
            agent.Speed = 3f;
            currAnim = AnimationType.UP_DOWN;

            sightRadius = 5f;
            scoreOnHitted = 800;
        }

        protected override void OnLeftWalking()
        {
            base.OnLeftWalking();

            if (ChangeAnim(AnimationType.LEFT))
            {
                sprite.FlipY = false;
            }
        }

        protected override void OnRightWalking()
        {
            base.OnRightWalking();

            if (ChangeAnim(AnimationType.RIGHT))
            {
                sprite.FlipY = false;
            }
        }

        protected override void OnUpWalking()
        {
            base.OnUpWalking();
        
            ChangeAnim(AnimationType.UP_DOWN);

            if (!sprite.FlipY)
                sprite.FlipY = true;
        }

        protected override void OnDownWalking()
        {
            base.OnDownWalking();

            ChangeAnim(AnimationType.UP_DOWN);

            if (sprite.FlipY)
                sprite.FlipY = false;
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
