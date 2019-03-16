using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Node
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Cost { get;  set; }  //era private
        public List<Node> Neighbours { get; private set; }

        public Node(int x, int y, int cost)
        {
            X = x;
            Y = y;
            Cost = cost;
            Neighbours = new List<Node>();
        }

        public void AddNeightbours(Node node)
        {
            Neighbours.Add(node);
        }

        public void RemoveNeightbours(Node node)
        {
            Neighbours.Remove(node);
        }

        public override string ToString()
        {
            return X + ", " + Y + " Cost: " + Cost;
        }
    }
}
