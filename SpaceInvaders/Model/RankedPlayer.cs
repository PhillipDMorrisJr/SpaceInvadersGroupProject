using System;


namespace SpaceInvaders.Model
{
    class RankedPlayer
    {
        private readonly int playerScore;
        private readonly int currentLevel;
        private readonly String playerIdentification;

        public enum Level
        {
            Level1 = 1, Level2, Level3
        }
   
        public RankedPlayer(int score, Level level) : this("Anonymous", score, level)
        {
        }

        public RankedPlayer(String name, int score, Level level)
        {
            this.playerScore = score;
            this.currentLevel = (int)level;
            this.playerIdentification = name + ": " + score;
        }


        #region Property

        public String PlayerID => this.playerIdentification;
        public int Score => this.playerScore;

        #endregion
    }
}
