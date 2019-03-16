using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class IndestructableObstacle : Obstacle
    {
        public IndestructableObstacle(Vector2 spritePosition, string spriteSheetName="ironWall") : base(spritePosition, spriteSheetName)
        {
            ObstacleManager.AddIndestructableObstaclePos(Position);
        }
    }
}
