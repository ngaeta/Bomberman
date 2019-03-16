using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class ExtraLifePowerUp : PowerUp
    {
        public ExtraLifePowerUp(Vector2 spritePosition) : base(spritePosition, "extraLife")
        {
            Type = PowerUpType.ExtraLife;
            clipOnPicked = AudioManager.GetAudioClip("powerUp3");
        }

        protected override void OnPlayerPick(Player p)
        {
            base.OnPlayerPick(p);
            p.HavingExtraLife = true;
        }

        protected override void OnPowerUpDeactive(Player p)
        {

        }
    }
}
