using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    static class BombsManager
    {
        const int NUMBER_OF_BOMBS = 1;

        private static Dictionary<Player, Queue<Bomb>> bombs;
        private static List<Player> playersToRemoveBomb;

        public static void Init()
        {
            bombs = new Dictionary<Player, Queue<Bomb>>();
            playersToRemoveBomb = new List<Player>();

            List<Player> players = PlayScene.Players;

            for (int j = 0; j < players.Count; j++)
            {
                Player player = players[j];
                bombs.Add(player, new Queue<Bomb>());

                for (int i = 0; i < NUMBER_OF_BOMBS; i++)
                {
                    bombs[player].Enqueue(new Bomb(Vector2.Zero, player));
                }
            }
        }

        public static void Update()
        {
            if(playersToRemoveBomb.Count > 0)
            {
                for(int i=0; i < playersToRemoveBomb.Count; i++)
                {
                    Player currPlayer = playersToRemoveBomb[i];

                    if(bombs[currPlayer].Count > 0)
                    {
                        bombs[currPlayer].Dequeue();
                        playersToRemoveBomb.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public static Bomb GetBomb(Player player)
        {
            if (bombs[player].Count > 0)
            {
                return bombs[player].Dequeue();
            }

            return null;
        }

        public static void EnqueBomb(Bomb bomb)
        {
            bombs[bomb.PlayerOwner].Enqueue(bomb);
        }

        public static void AddBombToQueue(Player player)
        {
            bombs[player].Enqueue(new Bomb(Vector2.Zero, player));
        }

        public static void RemoveBombFromQueue(Player player)
        {
            if (!playersToRemoveBomb.Contains(player))
                playersToRemoveBomb.Add(player);
        }

        public static void RemoveAll()
        {
            bombs.Clear();
            playersToRemoveBomb.Clear();
        }
    }
}
