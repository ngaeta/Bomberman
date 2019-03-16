using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class FireBurn : GameObject
    {
        public FireBurn(Vector2 spritePosition) : base(spritePosition, "fireBurn", DrawManager.Layer.Foreground)
        {
            IsActive = false;
        }

        public override void Update()
        {
            base.Update();

            if(IsActive && !Animation.IsPlaying)
            {
                IsActive = false;
            }
        }

        public void Play()
        {
            IsActive = true;
        }
    }
}
