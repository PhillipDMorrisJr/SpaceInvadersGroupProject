/// <summary>
/// 
/// </summary>

namespace SpaceInvaders.Model
{
    internal class Scoreboard
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Scoreboard" /> class.
        /// </summary>
        public Scoreboard()
        {
            Score = 0;
        }

        #endregion

        #region Properties

        #region Property

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; private set; }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        ///     Increase score based on the llevel of the object
        ///     Precondition: parameter "level" must be greater than 0
        ///     Postcondition: Score is increased based on the given formula
        /// </summary>
        public void IncreaseScore(int level)
        {
            if (level > 0)
            {
                var scoreValue = (level + 1)*100;
                Score += scoreValue;
            }
        }

        #endregion
    }
}