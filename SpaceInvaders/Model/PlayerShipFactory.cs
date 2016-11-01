using System;
using System.Collections.Generic;
using System.Linq;
using SpaceInvaders.Util;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Stores copies of a player ShipSelections
    /// </summary>
    public class PlayerShipFactory
    {
        #region Data members

        private const int AmountOfBufferShips = 1;
        private readonly List<PlayerShip> lives;
        #endregion

        #region Properties

        /// <summary>
        ///     Gets the amount of amountOfLives.
        /// </summary>
        /// <value>
        ///     The amount of amountOfLives.
        /// </value>
        public int AmountOfLives => this.lives.Count - AmountOfBufferShips;

        /// <summary>
        ///     Gets a value indicating whether this instance is there any amountOfLives.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is there any amountOfLives; otherwise, <c>false</c>.
        /// </value>
        public bool IsThereAnyLives => this.lives.Count > AmountOfBufferShips;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShipFactory" /> class.
        /// </summary>
        public PlayerShipFactory() : this(1)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShipFactory" /> class.
        /// </summary>
        /// <param name="amountOfLives">The amount of amountOfLives.</param>
        public PlayerShipFactory(int amountOfLives)
        {
            amountOfLives = addLifeWhenThereAreNoLives(amountOfLives);

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

        private void addLives(int amountOfLives)
        {
            PlayerShip initialPlayerShip = (PlayerShip)ShipFactory.SelectShip(ShipFactory.ShipSelections.PlayerShip);

            this.lives.Add(initialPlayerShip);

            for (var i = 0; i < amountOfLives; i++)
            {
                var aPlayerShip = (PlayerShip)ShipFactory.SelectShip(ShipFactory.ShipSelections.PlayerShip);
                this.lives.Add(aPlayerShip);
            }

            var bufferPlayerShip = (PlayerShip)ShipFactory.SelectShip(ShipFactory.ShipSelections.PlayerShip);
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
        ///     Uses the life and removes playerShip from PlayerShipFactory.
        ///     Precondition: there must be amountOfLives left.
        ///     Postcondition: There are less amountOfLives.
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
                throw new InvalidOperationException("There are no more amountOfLives.");
            }
        }

        #endregion
    }
}