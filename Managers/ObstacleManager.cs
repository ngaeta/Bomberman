using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    static class ObstacleManager
    {
        private static Dictionary<Vector2, bool> indestructableObstacles;

        static ObstacleManager()
        {
            indestructableObstacles = new Dictionary<Vector2, bool>();
        }

        public static void AddIndestructableObstaclePos(Vector2 pos)
        {
            indestructableObstacles.Add(pos, true);
        }

        public static bool IsThereObstacle(Vector2 pos)
        {
            return indestructableObstacles.ContainsKey(pos);
        }

        public static void RemoveAll()
        {
            indestructableObstacles.Clear();
        }
    }
}
