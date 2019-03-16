using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    //è un contenitore ordinato di elementi
    class PriorityQueue
    {
        private Dictionary<Node, int> items;
        public bool IsEmpty { get { return items.Count == 0; } }

        public PriorityQueue()
        {
            items = new Dictionary<Node, int>();
        }

        public void Enqueue(Node node, int priority)
        {
            items[node] = priority;
        }

        public Node Dequeue()
        {
            int lowerPriority = int.MaxValue;
            Node selectedNode = null;
            foreach(Node currNode in items.Keys)
            {
                int currPriority = items[currNode];
                if(currPriority < lowerPriority)
                {
                    lowerPriority = currPriority;
                    selectedNode = currNode;
                }                
            }
            items.Remove(selectedNode);
            return selectedNode;
        }
    }
}
