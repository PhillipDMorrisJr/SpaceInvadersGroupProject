using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Model
{
    /// <summary>
    /// Stores copies of a player Ship
    /// </summary>
    public class PlayerShipLives
    {
        #region Data members

        private const int AmountOfBufferShips = 1;
        private readonly List<PlayerShip> lives;
        private readonly int initialAmountOfLives;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the amount of lives.
        /// </summary>
        /// <value>
        /// The amount of lives.
        /// </value>
        public int AmountOfLives => this.lives.Count - AmountOfBufferShips;
        /// <summary>
        /// Gets a value indicating whether this instance is there any lives.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is there any lives; otherwise, <c>false</c>.
        /// </value>
        public bool IsThereAnyLives => this.lives.Count > AmountOfBufferShips;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerShipLives"/> class.
        /// </summary>
        public PlayerShipLives() : this(1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerShipLives"/> class.
        /// </summary>
        /// <param name="amountOfLives">The amount of lives.</param>
        public PlayerShipLives(int amountOfLives)
        {
            amountOfLives = addLifeWhenThereAreNoLives(amountOfLives);

            this.initialAmountOfLives = amountOfLives;
            this.lives = new List<PlayerShip>();
            this.addLives(amountOfLives);
        }

        #endregion

        #region Methods

        private static int addLifeWhenThereAreNoLives(int amountOfLives)
        {
            if (amountOfLives < 1)
            {
                amountOfLives = 1;
            }
            return amountOfLives;
        }

        private void addLives(int lives)
        {
            var initialPlayerShip = new PlayerShip();
            this.lives.Add(initialPlayerShip);

            for (var i = 0; i < lives; i++)
            {
                var aPlayerShip = new PlayerShip();
                this.lives.Add(aPlayerShip);
            }

            var bufferPlayerShip = new PlayerShip();
            this.lives.Add(bufferPlayerShip);
        }

        private void removeLifeInUse()
        {
            if (this.lives.Any())
            {
                var playerShipInUse = this.lives.First();
                this.lives.Remove(playerShipInUse);
            }
        }


        /// <summary>
        /// Uses the life and removes playerShip from PlayerShipLives.
        /// Precondition: there must be lives left.
        /// Postcondition: There are less lives.
        /// </summary>
        /// <returns>An unused playerShip</returns>
        public PlayerShip UseLife()
        {
            this.checkForLives();

            this.removeLifeInUse();

            return this.lives.First();
        }

        private void checkForLives()
        {
            if (!this.lives.Any())
            {
                throw new InvalidOperationException("There are no more lives.");
            }
        }

        #endregion
    }
}