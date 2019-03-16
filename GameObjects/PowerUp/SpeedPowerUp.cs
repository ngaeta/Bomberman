using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class SpeedPowerUp : PowerUp
    {
        public enum SpeedType { UP, DOWN }
        private float speedIncrement;

        public SpeedPowerUp(Vector2 spritePosition, SpeedType speedType) : base(spritePosition, "speedUp")
        {
            if (speedType == SpeedType.UP)
            {
                speedIncrement = 2f;
                Type = PowerUpType.SpeedUp;
                timeToDeactivePowerUp = 15f;
            }
            else
            {
                Type = PowerUpType.SpeedDown;
                speedIncrement = -2f;
                texture = GfxManager.GetSpritesheet("speedDown").Item1;
                timeToDeactivePowerUp = 10f;
                clipOnPicked = AudioManager.GetAudioClip("powerUp2");
            }
        }

        protected override void OnPlayerPick(Player p)
        {
            base.OnPlayerPick(p);
            p.AddSpeed(speedIncrement);
        }

        protected override void OnPowerUpDeactive(Player p)
        {
            base.OnPowerUpDeactive(p);
            p.AddSpeed(-speedIncrement);
        }
    }
}
