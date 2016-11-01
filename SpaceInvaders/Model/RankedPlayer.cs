namespace SpaceInvaders.Model
{
    internal class RankedPlayer
    {
        public enum Level
        {
            Level1 = 1,
            Level2,
            Level3
        }

        private readonly int currentLevel;

        public RankedPlayer(int score, Level level) : this("Anonymous", score, level)
        {
        }

        public RankedPlayer(string name, int score, Level level)
        {
            Score = score;
            currentLevel = (int) level;
            PlayerID = name + ": " + score;
        }

        #region Property

        public string PlayerID { get; }

        public int Score { get; }

        #endregion
    }
}