using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;
using Aiv.Audio;

namespace Bomberman
{
    class GameOverScene : FadeScene
    {
        enum Operation {Continue, Quit }

        private GUIItem guiBackground;
        private GUIItem gameOverLabel;
        private GUIItem gameOverBomberman;
        private List<GUIItem> guiToSelect;
        private GUIItem guiSelected;
        private int guiSelectedIndex;
        private float nextInput;

        public GameOverScene(float showTime = 3f, float fadeTime = 1.5f) : base(showTime, fadeTime)
        {
        }

        public override void Start()
        {
            base.Start();

            Vector2 center = new Vector2(Game.Window.OrthoWidth / 2, Game.Window.OrthoHeight / 2);
            guiBackground = new GUIItem(center, "blueBackground");
            gameOverBomberman = new GUIItem(center - new Vector2(0, Game.Window.OrthoHeight / 8), "gameOverBomberman");
            gameOverLabel = new GUIItem(center + new Vector2(0, Game.Window.OrthoHeight / 8), "gameOverLabel");

            guiToSelect = new List<GUIItem>();
            GUIItem continueGui = new GUIItem(gameOverLabel.Position + new Vector2(0, Game.Window.OrthoHeight / 8), "continue");
            GUIItem quitGui = new GUIItem(continueGui.Position + new Vector2(0, Game.Window.OrthoHeight / 8), "quit");
            guiToSelect.Add(continueGui);
            guiToSelect.Add(quitGui);
            guiSelectedIndex = 0;
            nextInput = 0;

            AddGameObjects(guiBackground);
            AddGameObjects(gameOverBomberman);
            AddGameObjects(gameOverLabel);
            AddGameObjects(continueGui);
            AddGameObjects(quitGui);
        }

        public override void Input()
        {
            if (statsScene == Stats.Show)
            {
                if (nextInput <= 0)
                {
                    if (Game.Window.GetKey(KeyCode.S))
                    {
                        nextInput = 0.5f;

                        guiSelectedIndex = (guiSelectedIndex + 1) % guiToSelect.Count;
                        SelectGUIOperation(guiSelectedIndex);
                    }
                    else if (Game.Window.GetKey(KeyCode.W))
                    {
                        nextInput = 0.5f;

                        guiSelectedIndex = Math.Abs((guiSelectedIndex - 1)) % guiToSelect.Count;
                        SelectGUIOperation(guiSelectedIndex);
                    }
                    else if (Game.Window.GetKey(KeyCode.Return))
                    {
                        CheckOperationSelected();
                    }
                }
            }
        }

        public override void Update()
        {
            if (statsScene == Stats.Show)
            {
                SelectGUIOperation(guiSelectedIndex);

                if (nextInput > 0)
                {
                    nextInput -= Game.DeltaTime;
                }
            }
            else
                base.Update();
        }

        public override void Draw()
        {
            base.Draw();
            DrawManager.Draw();
        }

        protected override void OnShowTimeStarted()
        {
            base.OnShowTimeStarted();

            AudioSource audioSource = new AudioSource();
            audioSource.Play(AudioManager.GetAudioClip("gameover"));
            AudioManager.DisposeAudioSource(audioSource);
        }

        private void SelectGUIOperation(int index)
        {
            if (guiSelected != null)
                guiSelected.GetSprite().SetMultiplyTint(Vector4.One);

            guiSelectedIndex = index;
            guiSelected = guiToSelect[index];
            guiSelected.GetSprite().SetMultiplyTint(0, 2, 0, 1);
        }

        private void CheckOperationSelected()
        {
            switch ((Operation)guiSelectedIndex)
            {
                case Operation.Continue:
                    Game.SceneToLoad = Game.SceneLoad.Prev;
                    break;
                case Operation.Quit:
                    Game.SceneToLoad = Game.SceneLoad.Next;
                    break;
            }

            statsScene = Stats.FadeOut;
            currTime = fadeTime;
        }

        public override void OnExit()
        {
            base.OnExit();
            statsScene = Stats.FadeIn;
        }
    }
}
