using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    abstract class Obstacle : GameObject
    {
        public Obstacle(Vector2 spritePosition, string spriteSheetName) : base(spritePosition, spriteSheetName, DrawManager.Layer.Playground)
        {
            Rect rect = new Rect(new Vector2(0.5f, 0.4f), null, Width, Height);

            RigidBody = new RigidBody(spritePosition, this, null, rect, false);
            RigidBody.Type = (uint) PhysicsManager.ColliderType.Obstacle;
        }
    }
}
