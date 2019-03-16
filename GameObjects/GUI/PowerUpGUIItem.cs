using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class PowerUpGUIItem : GUIItem
    {
        private static Vector4 colorDisabled = new Vector4(0.2f, 0.2f, 0.2f, 0.2f);

        public bool Enabled { get; private set; }
        public PowerUpType Type { get; private set; }

        public PowerUpGUIItem(Vector2 spritePosition, PowerUpType type) : base(spritePosition, "speedUp")
        {
            Type = type;
            Enabled = false;
            sprite.SetMultiplyTint(colorDisabled);

            switch (type)
            {
                //mettere costanti nelle classi
                case PowerUpType.SpeedUp:
                    texture = GfxManager.GetSpritesheet("speedUp").Item1;
                    break;
                case PowerUpType.SpeedDown:
                    texture = GfxManager.GetSpritesheet("speedDown").Item1;
                    break;
                case PowerUpType.BombUp:
                    texture = GfxManager.GetSpritesheet("bombUp").Item1;
                    break;
                case PowerUpType.BombDown:
                    texture = GfxManager.GetSpritesheet("bombDown").Item1;
                    break;
                case PowerUpType.Invincibility:
                    texture = GfxManager.GetSpritesheet("invincibility").Item1;
                    break;
                case PowerUpType.ExtraLife:
                    texture = GfxManager.GetSpritesheet("extraLife").Item1;
                    break;
            }
        }

        public void SwitchGUI()
        {
            if (!Enabled)
            {
                sprite.SetMultiplyTint(Vector4.One);
            }
            else
            {
                sprite.SetMultiplyTint(colorDisabled);
            }

            Enabled = !Enabled;
        }
    }
}
