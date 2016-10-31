using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace SpaceInvaders.Model
{
    class HighScoreBoard : Scoreboard
    {
        private List<RankedPlayer> highScores;
        private int AmountOfHighScores = 10;


        public HighScoreBoard()
        {
            this.highScores = new List<RankedPlayer>();
            //read files in parameters of highscores
        }

        public void AddPlayerTohighScoreBoard(String name, int score)
        {
            RankedPlayer aPlayer = new RankedPlayer(name, score);

           this.highScores.Add(aPlayer);

            this.sortHighScores();

            this.trimHighScoreCollection();
        }

        private void trimHighScoreCollection()
        {
            while (this.highScores.Count > this.AmountOfHighScores)
            {
                this.highScores.RemoveAt(9);
            }
        }

        private void sortHighScores()
        {
            var copyOfHighScores = this.highScores.OrderBy(player => player.Score);

            this.highScores.Clear();

            foreach (var variable in copyOfHighScores)
            {
                this.highScores.Add(variable);
            }
        }
    }
}
