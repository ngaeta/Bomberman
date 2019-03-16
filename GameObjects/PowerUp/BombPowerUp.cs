using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class BombPowerUp : PowerUp
    {
        public enum BombType {Up, Down}

        public BombPowerUp(Vector2 spritePosition, BombType bombType) : base(spritePosition, "bombUp")
        {
            if (bombType == BombType.Up)
            {
                Type = PowerUpType.BombUp;
                timeToDeactivePowerUp = 16f;
            }
            else
            {
                texture = GfxManager.GetSpritesheet("bombDown").Item1;
                Type = PowerUpType.BombDown;
                timeToDeactivePowerUp = 8f;
                clipOnPicked = AudioManager.GetAudioClip("powerUp2");
            }
        }

        protected override void OnPlayerPick(Player p)
        {
            base.OnPlayerPick(p);

            if (Type == PowerUpType.BombUp)
            {
                BombsManager.AddBombToQueue(p);
            }
            else
            {
                BombsManager.RemoveBombFromQueue(p);
            }
        }

        protected override void OnPowerUpDeactive(Player p)
        {
            base.OnPowerUpDeactive(p);

            if (Type == PowerUpType.BombUp)
            {
                BombsManager.RemoveBombFromQueue(p);
            }
            else
            {
                BombsManager.AddBombToQueue(p);
            }
        }
    }
}
