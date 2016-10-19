namespace SpaceInvaders.Model
{
    internal class Scoreboard
    {
        #region Data members

        #endregion

        #region Property

        public int Score { get; private set; }


        #endregion

        #region Constructors

        public Scoreboard()
        {
            this.Score = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///Increase score based on the llevel of the object
        /// Precondition: parameter "level" must be greater than 0
        /// Postcondition: Score is increased based on the given formula
        /// </summary>
        public void IncreaseScore(int level)
        {
            if (level > 0)
            {
                var scoreValue = (level + 1)*100;
                this.Score += scoreValue;
            }
        }

        #endregion
    }
}