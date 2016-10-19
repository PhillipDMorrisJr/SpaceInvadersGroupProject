using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Model
{
    public class PlayerShipLives
    {
        #region Data members

        private const int AmountOfBufferShips = 1;
        private readonly List<PlayerShip> lives;
        private readonly int initialAmountOfLives;

        #endregion

        #region Properties

        public int AmountOfLives => this.lives.Count - AmountOfBufferShips;
        public bool IsThereAnyLives => this.lives.Count > AmountOfBufferShips;

        #endregion

        #region Constructors

        public PlayerShipLives() : this(1)
        {
        }

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

        //returns next life in list
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