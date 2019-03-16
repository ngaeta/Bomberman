using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class InvinciblePowerUp : PowerUp
    {
        public InvinciblePowerUp(Vector2 spritePosition) : base(spritePosition, "invincibility")
        {
            Type = PowerUpType.Invincibility;
            timeToDeactivePowerUp = 10f;
            clipOnPicked = AudioManager.GetAudioClip("powerUp3");

        }

        protected override void OnPlayerPick(Player p)
        {
            base.OnPlayerPick(p);
            p.SetInvincibility(true);
        }

        protected override void OnPowerUpDeactive(Player p)
        {
            base.OnPowerUpDeactive(p);
            p.SetInvincibility(false);
        }
    }
}
