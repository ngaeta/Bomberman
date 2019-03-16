using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using System.Drawing;

namespace Bomberman
{
    class PlayScene : Scene
    {
        private float currTime = 1f;
        private bool debug;

        public static List<Player> Players { get; set; }
        public static Map Map { get; set;}
        public static List<Vector2> FreePositions { get; set; }

        public override void Start()
        {
            base.Start();

            Rect.Debug = false;
            Circle.Debug = false;
            PhysicsManager.RayDebug = false;

            PhysicsManager.Init();


            Players = new List<Player>();
            Players.Add(new WhiteBomberman(Vector2.Zero));
            Players.Add(new BlackBomberman(Vector2.Zero));

            Enemy.EnemyCount = 0;
            LevelGenerator.GenerateLevel();

            BombsManager.Init();
            GuiManager.Init(Players);
            GameManager.Init();
        }

        public override void Input()
        {
            GameManager.Input();
        }

        public override void Update()
        {
            GameManager.Update();
            AudioManager.Update();

            //if (Game.Window.GetKey(KeyCode.G))
            //{
            //    GameManager.NumOfEnemies--;
            //}
        }

        public override void Draw()
        {
            DrawManager.Draw();

            if(Game.Window.GetKey(KeyCode.Z))
                 Map.Draw();
        }

        public override void OnExit()
        {
            PhysicsManager.RemoveAll();
            UpdateManager.RemoveAll();
            DrawManager.RemoveAll();

            ObstacleManager.RemoveAll();
            BombsManager.RemoveAll();
            GuiManager.RemoveAll();

            AudioManager.RemoveAll();
        }

        public static void AddFreePositions(Vector2 pos)
        {
            FreePositions.Add(pos);
        }

        public static bool AreDeadAllPlayers()
        {
            for(int i=0; i < Players.Count; i++)
            {
                if(!Players[i].IsDead)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
