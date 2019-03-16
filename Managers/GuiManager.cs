using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bomberman
{
    static class GuiManager
    {
        static Dictionary<Player, GuiNumber> guiScores;
        static Dictionary<Player, List<GUIItem>> guiPowerUps;
        const int MAX_LENGHT_SCORE = 7;

        public static void Init(List<Player> players)
        {
            guiScores = new Dictionary<Player, GuiNumber>();
            guiPowerUps = new Dictionary<Player, List<GUIItem>>();

            for (int i = 0; i < players.Count; i++)
            {
                Player currPlayer = players[i];
                GUIItem guiImage = new GUIItem(currPlayer.GUIPosition, currPlayer.GUIImage);

                Vector2 scorePosition = guiImage.Position + new Vector2(0, guiImage.Height / 2) + new Vector2(-2, 2);
                guiScores[currPlayer] = new GuiNumber(scorePosition, "0000000");

                guiPowerUps[currPlayer] = new List<GUIItem>();
                int numPowerUps = Enum.GetValues(typeof(PowerUpType)).Length;
                Vector2 offset = new Vector2(0, 1);

                for (int j = 0; j < numPowerUps; j++)
                {
                    PowerUpType type = (PowerUpType)j;
                    GUIItem g = new PowerUpGUIItem(scorePosition + offset, type);
                    guiPowerUps[currPlayer].Add(g);
                    offset += new Vector2(1, 0);
                }
            }
        }

        public static void UpdateScores(Player player)
        {
            string playerScore = player.Score.ToString();

            if (playerScore.Length > MAX_LENGHT_SCORE)
            {
                playerScore = "9999999";
            }
            else
            {
                playerScore = player.Score.ToString("0000000");
            }

            guiScores[player].SetNumber(playerScore);
        }

        public static void SwitchGUIPowerUp(Player player, PowerUp powerUp, bool enable)
        {
            List<GUIItem> list = guiPowerUps[player];

            for (int i = 0; i < list.Count; i++)
            {
                PowerUpGUIItem powerUpGui = (PowerUpGUIItem)list[i];

                if (powerUp.Type == powerUpGui.Type && powerUpGui.Enabled != enable)
                {
                    powerUpGui.SwitchGUI();
                }
            }
        }

        public static void RemoveAll()
        {
            guiScores.Clear();
            guiPowerUps.Clear();
        }
    }
}
