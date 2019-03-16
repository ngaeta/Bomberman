using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace Bomberman
{
    class Explosion
    {
        const int NUMBER_FLAMES = 4;
        enum FlameType { UP, DOWN, RIGHT, LEFT }

        private Bomb owner;

        public Explosion(Vector2 position, Bomb owner)
        {
            this.owner = owner;

            float x = (float) Math.Round(position.X);
            float y = (float)Math.Round(position.Y);

            position = new Vector2(x, y);
            Flame centralFlame = new Flame(position, "centralFlame", owner);

            float heightFlame = centralFlame.Height;
            float widthFlame = centralFlame.Height;

            CreateFlames(FlameType.UP, position, new Vector2(0, -heightFlame));
            CreateFlames(FlameType.DOWN, position, new Vector2(0, heightFlame));
            CreateFlames(FlameType.RIGHT, position, new Vector2(widthFlame, 0));
            CreateFlames(FlameType.LEFT, position, new Vector2(-widthFlame, 0));
        }

        private void CreateFlames(FlameType type, Vector2 startPos, Vector2 offsetPos)
        {
            string flameName = "";
            string closeFlameName = "";

            switch (type)
            {
                case FlameType.UP:
                    flameName = "upFlame";
                    closeFlameName = "closeUpFlame";
                    break;
                case FlameType.DOWN:
                    flameName = "downFlame";
                    closeFlameName = "closeDownFlame";
                    break;
                case FlameType.RIGHT:
                    flameName = "leftRightFlame";
                    closeFlameName = "closeRightFlame";
                    break;
                case FlameType.LEFT:
                    flameName = "leftRightFlame";
                    closeFlameName = "closeLeftFlame";
                    break;
            }

            Vector2 flamePos = startPos + offsetPos;

            for (int i = 0; i < NUMBER_FLAMES; i++)
            {
                if (!ObstacleManager.IsThereObstacle(flamePos))
                {
                    if (i == NUMBER_FLAMES - 1 || ObstacleManager.IsThereObstacle(flamePos + offsetPos))
                    {
                        new Flame(flamePos, closeFlameName, owner);
                        break;
                    }
                    else
                    {                 
                        new Flame(flamePos, flameName, owner);
                        flamePos += offsetPos;
                    }
                }
                else 
                {             
                    break;
                }
            }
        }
    }
}
