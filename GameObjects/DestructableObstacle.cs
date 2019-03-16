using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class DestructableObstacle : Obstacle, IHittable
    {
        const int SCORE_ON_HITTED = 20;
        private bool isHitted;

        public DestructableObstacle(Vector2 spritePosition, string spriteSheetName = "brickWall") : base(spritePosition, spriteSheetName)
        {
        }

        bool IHittable.IsHitted()
        {
            return isHitted;
        }

        int IHittable.OnHit(GameObject hitter)
        {
            IsActive = false;
            isHitted = true;
            RigidBody.IsCollisionsAffected = false;

            GuiNumber guiNumber = new GuiNumber(Position, SCORE_ON_HITTED.ToString());
            guiNumber.Dissolve = true;
            guiNumber.Scale *= 0.4f;

            new FireBurn(Position).Play();

            PlayScene.Map.SetCost(Position, 1);
            PlayScene.FreePositions.Add(Position);

            Destroy();

            return SCORE_ON_HITTED;
        }
    }
}
