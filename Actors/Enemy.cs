using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace Bomberman
{
    public enum EnemyType { Fast, Strong, SuperView }

    abstract class Enemy : GameObject, IHittable
    {
        private Player currTargetPlayer;

        protected Agent agent;
        protected int indexAnimationDie;
        protected bool isDead;
        protected bool isHitted;
        protected bool playerSeen;
        protected float timeToDisappear;
        protected int scoreOnHitted;
        protected float sightRadius;
        protected Vector2 lookDirection;
        protected float halfConeAngle;
        protected float timeBeforeStopFollowPlayer = 5f;
        protected Vector4 colorMultiplierOnPlayerSeen;

        protected List<PhysicsManager.ColliderType> ignoreMaskRaySight;  //potrebbe servire

        public static int EnemyCount { get;  set; }

        public Enemy(Vector2 spritePosition, string spriteName) : base(spritePosition, spriteName, DrawManager.Layer.Foreground)
        {
            Rect rect = new Rect(new Vector2(sprite.Width / 2, sprite.Height / 2), null, sprite.Width, sprite.Height);
            RigidBody = new RigidBody(spritePosition, this, null, rect);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Enemy;
            RigidBody.SetCollisionMask((uint)PhysicsManager.ColliderType.Player);

            agent = new Agent(this);
            agent.Speed = 3;

            indexAnimationDie = animations.Count - 1;
            timeToDisappear = 1.8f;

            playerSeen = false;
            lookDirection = new Vector2(0, 1);
            sightRadius = 10f;
            halfConeAngle = MathHelper.DegreesToRadians(60);
            colorMultiplierOnPlayerSeen = new Vector4(2, 1, 1, 1);

            ignoreMaskRaySight = new List<PhysicsManager.ColliderType>();
            ignoreMaskRaySight.Add(PhysicsManager.ColliderType.Enemy);

            scoreOnHitted = 100;
            EnemyCount++;
        }

        public override void Update()
        {
            base.Update();

            if (!isDead)
            {
                if (CheckPlayerInFov())
                {
                    int startX = (int)Position.X;
                    int startY = (int)Position.Y;
                    int endX = (int)currTargetPlayer.Position.X;
                    int endY = (int)currTargetPlayer.Position.Y;

                    List<Node> pathToPlayer = PlayScene.Map.GetPath(startX, startY, endX, endY);

                    if (pathToPlayer.Count > 0)
                    {
                        timeBeforeStopFollowPlayer = 5f;
                        playerSeen = true;
                        OnPlayerSeen();
                        agent.SetPath(pathToPlayer);
                    }
                }
                else if (playerSeen)
                {
                    if (timeBeforeStopFollowPlayer <= 0)
                    {
                        playerSeen = false;
                        currTargetPlayer = null;
                        OnPlayerLost();

                        agent.ResetPath();
                    }
                    else
                        timeBeforeStopFollowPlayer -= Game.DeltaTime;
                }

                if (playerSeen == false && agent.Target == null)
                {
                    //seleziona un percorso casuale utilizzando freepositions nella playscene
                    List<Vector2> freePositions = PlayScene.FreePositions;
                    int indexRandom = RandomGenerator.GetRandom(0, freePositions.Count);
                    Vector2 randomPos = freePositions[indexRandom];

                    int startX = (int)Position.X;
                    int startY = (int)Position.Y;
                    int endX = (int)randomPos.X;
                    int endY = (int)randomPos.Y;

                    List<Node> randomPath = PlayScene.Map.GetPath(startX, startY, endX, endY);
                    agent.SetPath(randomPath);
                }

                agent.Update();

                CheckAgentDirection();
            }
            else
            {
                if (timeToDisappear <= 0)
                {
                    OnDie();
                }
                else
                    timeToDisappear -= Game.DeltaTime;
            }
        }

        public override void OnCollide(Collision collision)
        {
            base.OnCollide(collision);

            if (collision.collider is IHittable h)
            {
                h.OnHit(this);
            }
        }

        int IHittable.OnHit(GameObject hitter)
        {
            return OnHit();
        }

        protected virtual int OnHit()
        {
            isDead = true;
            isHitted = true;
            RigidBody.IsCollisionsAffected = false;

            Animation = animations[indexAnimationDie];
            Animation.Play();

            return scoreOnHitted;
        }

        protected virtual void OnLeftWalking()
        {
            lookDirection = new Vector2(-1, 0);
        }

        protected virtual void OnRightWalking()
        {
            lookDirection = new Vector2(1, 0);
        }

        protected virtual void OnUpWalking()
        {
            lookDirection = new Vector2(0, -1);
        }

        protected virtual void OnDownWalking()
        {
            lookDirection = new Vector2(0, 1);
        }

        protected virtual void OnPlayerSeen()
        {
            sprite.SetMultiplyTint(colorMultiplierOnPlayerSeen);
        }

        protected virtual void OnPlayerLost()
        {
            sprite.SetMultiplyTint(Vector4.One);
        }

        private void CheckAgentDirection()
        {
            if (agent.Direction.X < 0)
            {
                OnLeftWalking();
            }
            else if (agent.Direction.X > 0)
            {
                OnRightWalking();
            }
            else if (agent.Direction.Y < 0)
            {
                OnUpWalking();
            }
            else if (agent.Direction.Y > 0)
            {
                OnDownWalking();
            }
        }

        private void OnDie()
        {
            IsActive = false;

            GuiNumber guiNumber = new GuiNumber(Position, scoreOnHitted.ToString());
            guiNumber.Dissolve = true;
            guiNumber.Scale *= 0.4f;

            new DisappearEnemyEffect(Position);

            EnemyCount--;
            Destroy();
        }

        private bool CheckPlayerInFov()
        {
            bool playerSeen = false;

            for (int i = 0; i < PlayScene.Players.Count; i++)
            {
                Player currPlayer = PlayScene.Players[i];

                if(currPlayer.IsDead && currPlayer == currTargetPlayer)
                {
                    OnPlayerLost();
                }

                if (!currPlayer.IsDead)
                {
                    Vector2 distance = currPlayer.Position - Position;

                    if (Math.Abs(distance.Length) <= sightRadius)
                    {
                        Vector2 distDirection = distance.Normalized();

                        float dot = Vector2.Dot(distDirection, lookDirection);
                        float deltaAngle = (float)Math.Acos(dot);

                        if (dot > 0 && deltaAngle <= halfConeAngle)
                        {
                            Tuple<RigidBody, float> intersection = PhysicsManager.RayCast(Position, distDirection, RigidBody, distance.Length, ignoreMaskRaySight);

                            if (intersection.Item1 == currPlayer.RigidBody)
                            {
                                playerSeen = true;

                                if (currTargetPlayer == null)
                                {
                                    currTargetPlayer = currPlayer;
                                }
                                else if (currTargetPlayer != currPlayer)
                                {
                                    Vector2 distanceFromPlayerTarget = currTargetPlayer.Position - Position;
                                    Vector2 distanceFromCurrentPlayer = currPlayer.Position - Position;

                                    if (distanceFromCurrentPlayer.Length < distanceFromPlayerTarget.Length)
                                    {
                                        currTargetPlayer = currPlayer;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return playerSeen;
        }

        bool IHittable.IsHitted()
        {
            return isHitted;
        }
    }
}
