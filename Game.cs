using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Bomberman
{
    static class Game
    {
        public enum SceneLoad { Next, Prev }
        private static Window window;
        private static float unitSize;

        public static Window Window { get { return window; } }
        public static float DeltaTime { get { return window.deltaTime; } }
        public static float Gravity { get { return 0; } }
        public static Scene CurrScene { get; set; }
        public static SceneLoad SceneToLoad {get; set;}

        static Game()
        {
            window = new Window(1280, 720, "Bomberman")
            {
                Position = new Vector2(300, 50)
            };

            window.SetDefaultOrthographicSize(30);
            window.SetIcon("Assets/bomberMan.ico");
            unitSize = window.Height / window.CurrentOrthoGraphicSize;
        }

        public static void Play()
        {
            GfxManager.Load();
            AudioManager.Load();

            Scene playScene = new PlayScene();
            Scene gameOverScene = new GameOverScene();

            playScene.NextScene = gameOverScene;
            gameOverScene.PreviousScene = playScene;

            CurrScene = playScene;
            CurrScene.Start();

            while (Window.IsOpened)
            {
                //float fps = 1 / Window.deltaTime;
                //Console.SetCursorPosition(0, 0);
                //if (fps < 59)
                //    Console.Write((1 / Window.deltaTime) + "                   ");

                if (Window.GetKey(KeyCode.Esc))
                    break;

                if (!CurrScene.IsPlaying)
                {
                    if (SceneToLoad == SceneLoad.Next)
                    {
                        if (CurrScene.NextScene != null)
                        {
                            CurrScene.OnExit();
                            CurrScene = CurrScene.NextScene;
                            CurrScene.Start();
                        }
                        else
                            return;
                    } else
                    {
                        if (CurrScene.PreviousScene != null)
                        {
                            CurrScene.OnExit();
                            CurrScene = CurrScene.PreviousScene;
                            CurrScene.Start();
                            SceneToLoad = SceneLoad.Next;
                        }
                        else
                            return;
                    }
                }

                CurrScene.Input();
                CurrScene.Update();
                CurrScene.Draw();

                Window.Update();
            }
        }

        public static float PixelsToUnit(float size)
        {
            return size / unitSize;
        }
    }
}
