using System;

namespace SpaceInvaders.Util
{
    /// <summary>
    ///     Defines static Random object.
    /// </summary>
    public static class RandomUtil
    {
        private static readonly Random random = new Random();

        /// <summary>
        ///     Gets the next random.
        /// </summary>
        /// <returns></returns>
        public static int GetNextRandom()
        {
            return random.Next();
        }

        /// <summary>
        ///     Gets the next random from range.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns></returns>
        public static int GetNextRandomFromRange(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
    }
}