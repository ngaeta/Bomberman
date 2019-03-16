using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class StrongEnemy : Enemy
    {
        const float TIME_INVULNERABILITY = 3f;

        private int lives;
        private bool isHitted;
        private float currTimeInvulnerability;
        private float timeBlink;

        public StrongEnemy(Vector2 spritePosition) : base(spritePosition, "purpleEnemy")
        {
            lives = 2;
            currTimeInvulnerability = TIME_INVULNERABILITY;
            isHitted = false;
            scoreOnHitted = 500;

            agent.Speed = 1.5f;
        }

        public override void Update()
        {
            base.Update();

            if(isHitted)
            {
                if (currTimeInvulnerability > 0)
                {
                    currTimeInvulnerability -= Game.DeltaTime;
                    timeBlink += Game.DeltaTime * 30;

                    float multiply = (float)Math.Cos(timeBlink);
                    sprite.SetMultiplyTint(new Vector4(multiply, multiply, multiply, multiply));
                }
                else
                {
                    isHitted = false;
                    timeBlink = 0;
                    currTimeInvulnerability = TIME_INVULNERABILITY;
                    sprite.SetMultiplyTint(Vector4.One);
                }
            }
        }

        protected override int OnHit()
        {
            if (!isHitted && --lives <= 0)
                return base.OnHit();
            else
            {
                isHitted = true;
                return 0;
            }
        }

        protected override void OnPlayerSeen()
        {
            if(!isHitted)
                base.OnPlayerSeen();
        }

        protected override void OnPlayerLost()
        {
            if(!isHitted)
                base.OnPlayerLost();
        }
    }
}
