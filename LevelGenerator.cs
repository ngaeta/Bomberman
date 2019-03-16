using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    static class LevelGenerator
    {
        const int NUM_ENEMIES = 10;
        static Vector2[] playersPosition;
        static int xStartingPos;
        static int xEndingPos;

        static LevelGenerator()
        {
            xStartingPos = (int)Game.Window.OrthoWidth / 5;
            xEndingPos = (int)Game.Window.OrthoWidth - xStartingPos;

            playersPosition = new Vector2[4];
            playersPosition[0] = new Vector2(xStartingPos + 2, 2);
            playersPosition[1] = new Vector2(xEndingPos - 2, Game.Window.OrthoHeight - 2);
        }

        public static void GenerateLevel()
        {
            GenerateBackground();
            GenerateBorderWalls();
            SetPlayersPosition();

            List<Vector2> freePositions = new List<Vector2>();

            //grass generation and freePositions
            for (int i = 1; i < Game.Window.OrthoHeight - 1; i++)
            {
                for (int j = xStartingPos + 1; j < xEndingPos - 1; j++)
                {
                    Vector2 pos = new Vector2(j, i);
                    new GameObject(pos, "grass2", DrawManager.Layer.Background);

                    freePositions.Add(pos);
                }
            }

            int[] cells = new int[freePositions.Count];
            Dictionary<Vector2, int> dictPositionIndex = new Dictionary<Vector2, int>();

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = 1;
                dictPositionIndex[freePositions[i]] = i;
            }

            for (int k = 0; k < PlayScene.Players.Count; k++)
            {
                Vector2 playerPos = PlayScene.Players[k].Position;

                freePositions.Remove(playerPos);
                freePositions.Remove(playerPos + new Vector2(1, 0));
                freePositions.Remove(playerPos + new Vector2(-1, 0));
                freePositions.Remove(playerPos + new Vector2(0, 1));
                freePositions.Remove(playerPos + new Vector2(0, -1));
                freePositions.Remove(playerPos + new Vector2(2, 0));
                freePositions.Remove(playerPos + new Vector2(-2, 0));
                freePositions.Remove(playerPos + new Vector2(0, 2));
                freePositions.Remove(playerPos + new Vector2(0, -2));
                freePositions.Remove(playerPos + new Vector2(3, 0));
                freePositions.Remove(playerPos + new Vector2(-3, 0));
                freePositions.Remove(playerPos + new Vector2(0, 3));
                freePositions.Remove(playerPos + new Vector2(0, -3));
            }

            GenerateRandomWalls(freePositions, ref cells, dictPositionIndex);
            GenerateEnemies(freePositions);
            GeneratePickable(new List<Vector2>(freePositions)); //We pass a copy because it gave problems with remove

            List<Tuple<Vector2, int>> list = new List<Tuple<Vector2, int>>();
            freePositions.Clear();

            foreach (KeyValuePair<Vector2, int> pair in dictPositionIndex)
            {
                list.Add(new Tuple<Vector2, int>(pair.Key, cells[pair.Value]));

                if (pair.Value > 0)
                {
                    freePositions.Add(pair.Key);
                }
            }

            Vector2 startPos = new Vector2(11, 1);
            Vector2 endPos = new Vector2(41, 28);

            PlayScene.Map = new Map(xEndingPos - xStartingPos - 3, (int)Game.Window.OrthoHeight - 2, list, startPos, endPos);
            PlayScene.FreePositions = freePositions;
        }

        private static void SetPlayersPosition()
        {
            for (int i = 0; i < PlayScene.Players.Count; i++)
            {
                PlayScene.Players[i].Position = playersPosition[i];
            }
        }

        private static void GenerateBackground()
        {
            GameObject background = new GameObject(new Vector2(Game.Window.OrthoWidth / 2, Game.Window.OrthoHeight / 2), "background", DrawManager.Layer.Background);
            background.GetSprite().scale *= 3;
            //background.SetPivot(new Vector2(background.Width / 2, background.Height / 2));
        }

        private static void GenerateBorderWalls()
        {
            //horizontal border game
            for (int i = xStartingPos; i < xEndingPos; i++)
            {
                new IndestructableObstacle(new Vector2(i, 0));
                new IndestructableObstacle(new Vector2(i, Game.Window.OrthoHeight - 1));
            }

            //vertical border game
            for (int i = 1; i < Game.Window.OrthoHeight - 1; i++)
            {
                new IndestructableObstacle(new Vector2(xStartingPos, i));
                new IndestructableObstacle(new Vector2(xEndingPos - 1, i));
            }
        }

        private static void GenerateRandomWalls(List<Vector2> freePositions, ref int[] cells, Dictionary<Vector2, int> dict)
        {
            int numberWallsDestructible = RandomGenerator.GetRandom(freePositions.Count / 5, freePositions.Count / 4);
            int n1 = 0;
            int widthMap = xEndingPos - xStartingPos - 3;

            for (; numberWallsDestructible > 0; numberWallsDestructible--)
            {
                if (numberWallsDestructible > freePositions.Count)
                    break;

                int indexRandomPosition = RandomGenerator.GetRandom(0, freePositions.Count);
                Vector2 pos = freePositions[indexRandomPosition];
                new DestructableObstacle(pos);
                n1++;

                freePositions.RemoveAt(indexRandomPosition);
                cells[dict[pos]] = 0;

                if (indexRandomPosition + 1 < freePositions.Count)
                {
                    freePositions.RemoveAt(indexRandomPosition + 1);
                }

                if (indexRandomPosition - 1 > 0)
                {
                    freePositions.RemoveAt(indexRandomPosition - 1);  //per far spazio dopo e prima
                }

                //if (indexRandomPosition - xEndingPos > 0)
                //{
                //    freePositions.RemoveAt(indexRandomPosition - xEndingPos);
                //}

                //if (indexRandomPosition + xEndingPos < freePositions.Count)
                //{
                //    freePositions.RemoveAt(indexRandomPosition + xEndingPos);
                //}
            }

            Console.WriteLine("muri distruttibili generati: " + n1);

            int numberWallsIndestructible = RandomGenerator.GetRandom(freePositions.Count / 4, freePositions.Count / 3);
            n1 = 0;

            for (; numberWallsIndestructible > 0; numberWallsIndestructible--)
            {
                int indexRandomPosition = RandomGenerator.GetRandom(0, freePositions.Count);
                Vector2 pos = freePositions[indexRandomPosition];

                new IndestructableObstacle(pos);
                n1++;

                freePositions.RemoveAt(indexRandomPosition);
                cells[dict[pos]] = 0;

                if (indexRandomPosition + 1 < freePositions.Count)
                    freePositions.RemoveAt(indexRandomPosition + 1);

                if (indexRandomPosition - 1 > 0)
                {
                    freePositions.RemoveAt(indexRandomPosition - 1);  //per far spazio dopo e prima
                }

                //if(indexRandomPosition - xEndingPos > 0)
                //{
                //    freePositions.RemoveAt(indexRandomPosition - xEndingPos);
                //}

                //if(indexRandomPosition + xEndingPos < freePositions.Count)
                //{
                //    freePositions.RemoveAt(indexRandomPosition + xEndingPos);
                //}
            }

            Console.WriteLine("muri indistruttibili generati: " + n1);

            Console.WriteLine("mappa");

            for (int i = 0; i < cells.Length; i++)
            {
                if (i % (widthMap + 1) == 0)
                {
                    Console.WriteLine();
                }

                Console.Write(cells[i]);
            }
        }

        private static void GenerateEnemies(List<Vector2> freePositions)
        {
            //int numEnemies = RandomGenerator.GetRandom(8, 12);

            for (int i = 0; i < NUM_ENEMIES; i++)
            {
                int randomEnemy = RandomGenerator.GetRandom(0, 3);
                Vector2 randomPos = freePositions[RandomGenerator.GetRandom(0, freePositions.Count)];

                switch ((EnemyType)randomEnemy)
                {
                    case EnemyType.Fast:
                        new FastEnemy(randomPos);
                        break;
                    case EnemyType.Strong:
                        new StrongEnemy(randomPos);
                        break;
                    case EnemyType.SuperView:
                        new SuperViewEnemy(randomPos);
                        break;
                }
            }
        }
        private static void GeneratePickable(List<Vector2> freePositions)
        {
            int num = RandomGenerator.GetRandom(15, 20);
            int numPowerUpsType = Enum.GetValues(typeof(PowerUpType)).Length;

            for (int i = 0; i < num; i++)
            {
                if (freePositions.Count <= 0)
                    return;

                int randomPowerUp = RandomGenerator.GetRandom(0, numPowerUpsType);
                int randomIndex = RandomGenerator.GetRandom(0, freePositions.Count);
                Vector2 randomPos = freePositions[randomIndex];
                freePositions.RemoveAt(randomIndex);

                switch ((PowerUpType)randomPowerUp)
                {
                    case PowerUpType.SpeedUp:
                        new SpeedPowerUp(randomPos, SpeedPowerUp.SpeedType.UP);
                        break;
                    case PowerUpType.SpeedDown:
                        new SpeedPowerUp(randomPos, SpeedPowerUp.SpeedType.DOWN);
                        break;
                    case PowerUpType.BombUp:
                        new BombPowerUp(randomPos, BombPowerUp.BombType.Up);
                        break;
                    case PowerUpType.BombDown:
                        new BombPowerUp(randomPos, BombPowerUp.BombType.Down);
                        break;
                    case PowerUpType.Invincibility:
                        new InvinciblePowerUp(randomPos);
                        break;
                    case PowerUpType.ExtraLife:
                        new ExtraLifePowerUp(randomPos);
                        break;
                }
            }

            int randomNumberScorable = RandomGenerator.GetRandom(2, 6);

            for (int i = 0; i < randomNumberScorable; i++)
            {
                if (freePositions.Count <= 0)
                    return;

                int randomIndx = RandomGenerator.GetRandom(0, freePositions.Count);
                Vector2 randomPosition = freePositions[randomIndx];
                freePositions.RemoveAt(randomIndx);

                new ObjectScore(randomPosition);
            }
        }
    }
}
