using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    class Scoreboard
    {
        private int score;
        public Scoreboard()
        {
            this.score = 0;
        }

        //increase score based on the llevel of the object
        public void increaseScore(int level)
        {
            int scoreValue = (level + 1)*100;
            this.score += scoreValue;
        }
        #region Property

        public int Score => this.score;
        #endregion]

    }
}
