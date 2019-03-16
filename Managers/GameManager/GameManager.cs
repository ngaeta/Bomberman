using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    static class GameManager
    {
        public enum GameState {CountDown, Play, Win, Lose}

        private static StateMachine stateMachine;

        public static void Init()
        {
            State countDownState = new CountDownState();
            State playState = new PlayState();
            State winState = new WinState();
            State loseState = new LoseState();

            stateMachine = new StateMachine();
            stateMachine.RegisterState((int) GameState.CountDown, countDownState);
            stateMachine.RegisterState((int)GameState.Play, playState);
            stateMachine.RegisterState((int)GameState.Win, winState);
            stateMachine.RegisterState((int)GameState.Lose, loseState);

            stateMachine.Switch((int)GameState.CountDown);
        }

        public static void Input()
        {
            stateMachine.Input();
        }

        public static void Update()
        {
            stateMachine.Run();
        }
    }
}
