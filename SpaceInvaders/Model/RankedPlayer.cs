using System;


namespace SpaceInvaders.Model
{
    class RankedPlayer
    {
        private readonly int playerScore;
        private readonly String playerIdentification;

   
        public RankedPlayer(int score) : this("Anonymous", score)
        {
        }

        public RankedPlayer(String name, int score)
        {
            this.playerScore = score;
            this.playerIdentification = name + ": " + score;
        }


        #region Property

        public String PlayerID => this.playerIdentification;
        public int Score => this.playerScore;

        #endregion
    }
}
