using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace Bomberman
{
    class PlayState : State
    {
        private AudioSource audioSource;
        private GUIItem startLabel;
        private float timeToDeactiveStartLabel;

        public override void Enter()
        {
            base.Enter();

            startLabel = new GUIItem(new OpenTK.Vector2(Game.Window.OrthoWidth / 2, Game.Window.OrthoHeight / 2), "start");
            timeToDeactiveStartLabel = 1.5f;

            audioSource = new AudioSource();
            audioSource.Play(AudioManager.GetAudioClip("start"));

            AudioManager.SetBackgroundAudio("backgroundMusic");
            AudioManager.DisposeAudioSource(audioSource);
        }

        public override void Input()
        {
            base.Input();

            for (int i = 0; i < PlayScene.Players.Count; i++)
                PlayScene.Players[i].Input();
        }

        public override void Update()
        {
            base.Update();

            PhysicsManager.Update();
            UpdateManager.Update();
            PhysicsManager.CheckCollisions();
            BombsManager.Update();

            //enemyCount in class Enemy is better
            if(Enemy.EnemyCount <= 0)
            {
               machine.Switch((int)GameManager.GameState.Win);
            }
            else if(PlayScene.AreDeadAllPlayers())
            {
                machine.Switch((int)GameManager.GameState.Lose);
            }

            if (startLabel.IsActive)
            {
                if (timeToDeactiveStartLabel <= 0)
                {
                    startLabel.IsActive = false;
                }
                else
                    timeToDeactiveStartLabel -= Game.DeltaTime;
            }
        }
    }
}
