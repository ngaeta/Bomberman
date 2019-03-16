using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Bomberman
{
    class Map
    {
        private Vector2 startPos;
        private Vector2 endPos;
        private int width;
        private int height;
        private Sprite sprite;
        private Node[] nodes;
        private Dictionary<Vector2, Node> nodesDict;

        private Dictionary<Node, Node> cameFrom; //sarebbe il vettore dei padri

        public Map(int width, int height, List<Tuple<Vector2, int>> list, Vector2 startPos, Vector2 endPos)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.width = width;
            this.height = height;

            sprite = new Sprite(1, 1);
            nodes = new Node[list.Count];
            nodesDict = new Dictionary<Vector2, Node>();

            for (int i = 0; i < list.Count; i++)
            {
                int x = (int)list[i].Item1.X;
                int y = (int)list[i].Item1.Y;
                nodes[i] = new Node(x, y, list[i].Item2);
                nodesDict[new Vector2(x, y)] = nodes[i];
            }

            foreach(Node n in nodes)
            {
                CheckNeighbour(n);
            }
        }

        private void CheckNeighbour(Node currNode) //passiamo il node padre e l'indice del vicino
        {
            Vector2 topPosition, bottomPos, rightPos, leftPos;

            topPosition = new Vector2(currNode.X, currNode.Y - 1);
            bottomPos = new Vector2(currNode.X, currNode.Y + 1);
            rightPos = new Vector2(currNode.X + 1, currNode.Y);
            leftPos = new Vector2(currNode.X - 1, currNode.Y);

            if(topPosition.Y >= startPos.Y)
            {
                currNode.AddNeightbours(nodesDict[topPosition]);
            }

            if(bottomPos.Y <= endPos.Y)
            {
                currNode.AddNeightbours(nodesDict[bottomPos]);
            }

            if (leftPos.X >= startPos.X)
            {
                currNode.AddNeightbours(nodesDict[leftPos]);
            }

            if (rightPos.X <= endPos.X)
            {
                currNode.AddNeightbours(nodesDict[rightPos]);
            }
        }

        private int Heuristic(Node start, Node end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }

        private void AStar(Node start, Node end)
        {
            Dictionary<Node, int> costSoFar; //sarebbe l'array delle distanze
            PriorityQueue frontier; //sarebbe la coda

            frontier = new PriorityQueue();
            cameFrom = new Dictionary<Node, Node>();
            costSoFar = new Dictionary<Node, int>();

            cameFrom[start] = start;
            costSoFar[start] = 0;
            frontier.Enqueue(start, Heuristic(start, end));

            while (!frontier.IsEmpty)
            {
                Node currNode = frontier.Dequeue();
                if (currNode == end)
                {
                    return;
                }
                foreach (Node nextNode in currNode.Neighbours)
                {
                    if (nextNode.Cost <= 0)
                        continue;

                    int newCost = costSoFar[currNode] + nextNode.Cost;
                    if (!costSoFar.ContainsKey(nextNode) || costSoFar[nextNode] > newCost)
                    {
                        costSoFar[nextNode] = newCost;
                        cameFrom[nextNode] = currNode;
                        frontier.Enqueue(nextNode, newCost + Heuristic(nextNode, end));
                    }
                }
            }
        }

        //1-a partire dalle coordinate ricavi i nodi start e end
        //2-utilizza a star per riempire il dictionary "cameFrom"
        //3-calcola il path da percorrere a partire dal "cameFrom"
        //4-ritorna il path
        public List<Node> GetPath(int startX, int startY, int endX, int endY) //coordinate da dove parte l'agent e dove vuole arricare
        {
            List<Node> path = new List<Node>();

            Node start = nodesDict[new Vector2(startX, startY)];

            Vector2 pos = new Vector2(endX, endY);

            if(!nodesDict.ContainsKey(pos))
            {
                return path;
            }

            Node end = nodesDict[pos];

            if (start == null || end == null)
            {
                return path;
            }

            AStar(start, end);

            Node currNode = end;

            if (!cameFrom.ContainsKey(currNode))
            {
                return path;
            }

            while (currNode != cameFrom[currNode])
            {
                path.Add(currNode);
                currNode = cameFrom[currNode];
            }
            path.Reverse(); //inverte la lista

            return path;
        }

        public void SetCost(Vector2 position, int newCost)
        {
            nodesDict[position].Cost = newCost;
        }

        //1-Disegna la mappa
        //2-le celle == 0 sono nere
        //3-altrimeti sono rosse
        public void Draw()
        {
            foreach (Node n in nodes)
            {
                sprite.position = new Vector2(n.X, n.Y);

                if (n == null)
                {
                    sprite.DrawSolidColor(0, 0, 0);
                }
                else
                {
                    if (n.Cost > 0)
                    {
                        sprite.DrawSolidColor(255, 255, 255);
                    }
                    else
                    {
                        sprite.DrawSolidColor(255, 0, 0);
                    }
                }
            }
        }
    }
}
