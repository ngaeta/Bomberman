using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    class Flame : GameObject
    {
        private Bomb ownerBomb;

        public Flame(Vector2 spritePosition, string textureName, Bomb ownerBomb) : base(spritePosition, textureName, DrawManager.Layer.Foreground)
        {
            this.ownerBomb = ownerBomb;

            Rect rect = new Rect(new Vector2(0.6f, 0.6f), null, Width, Height);
            RigidBody = new RigidBody(spritePosition, this, null, rect, false);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Flame;
            RigidBody.SetCollisionMask((uint) (PhysicsManager.ColliderType.Player | PhysicsManager.ColliderType.Obstacle | PhysicsManager.ColliderType.Enemy | PhysicsManager.ColliderType.Pickable));

            Animation.ReverseAfterFinish = true;
            sprite.scale *= 1.2f;
        }

        public override void Update()
        {
            base.Update();

            if(IsActive && !Animation.IsPlaying)
            {
                IsActive = false;
            }
        }

        public override void OnCollide(Collision collision)
        {
            base.OnCollide(collision);

            if(collision.collider is IHittable hitObject && !hitObject.IsHitted())
            {
                int score = hitObject.OnHit(this);

                if(score != 0 && collision.collider != ownerBomb.PlayerOwner)
                {
                    ownerBomb.PlayerOwner.AddScore(score);
                }
            }
        }
    }
}
