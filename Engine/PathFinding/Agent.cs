using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Bomberman
{
    class Agent
    {
        //accetta un path da seguire
        //segue il path
        protected Vector2 dir;
        public GameObject Owner { get; set; }
        protected List<Node> path;
        public Node Target { get; private set; }
      
        //converts the position to the nearest int (example 0.99.09 => 1)
        //with the casting it would be 0
        public int X { get { return Convert.ToInt32(Owner.Position.X); } }
        public int Y { get { return Convert.ToInt32(Owner.Position.Y); } }
        public float Speed { get; set; }
        public Node currNode;
        public Vector2 Direction { get { return dir; } }

        public Agent(GameObject owner) : this(owner, 3f)
        {

        }

        protected Agent(GameObject owner, float speed)
        {
            Owner = owner;
            Target = null;
            Speed = speed;
        }

        public void SetPath(List<Node> path)
        {
            this.path = path;

            if (Target == null && path.Count > 0)
            {
                Target = path[0];
                this.path.RemoveAt(0);
            }
            else if(path.Count > 0)
            {
                int manDist = Math.Abs(Target.X - path[0].X) + Math.Abs(Target.Y - path[0].Y);
                if(manDist > 1)
                {
                    path.Insert(0, currNode);
                }
            }
        }

        public virtual void Update()
        {
            if(Target != null)
            {
                Vector2 dest = new Vector2(Target.X, Target.Y);
                dir = dest - Owner.Position; //vettore che ci dice la distanza
                float dist = dir.Length;

                if(dist < 0.1f)
                {
                    currNode = Target;
                    Owner.Position = dest;
                    //we arrived to destination
                    if(path.Count == 0)
                    {
                        OnDestinationArrived();
                    }
                    else
                    {
                        Target = path[0];
                        path.RemoveAt(0);
                    }
                }

                else
                {
                    Owner.Position += dir.Normalized() * Game.Window.deltaTime * Speed;
                }
            }
        }

        public void ResetPath()
        {
            if(path != null)
            {
                path.Clear();
            }

            Target = null;
        }

        public Node GetLastNode()
        {
            if(path.Count > 0)
            {
                return path.Last();
            }

            return null;
        }

        protected virtual void OnDestinationArrived()
        {
            Target = null;
        }
    }
}
