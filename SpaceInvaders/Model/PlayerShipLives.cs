using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public class PlayerShipLives
    {
        #region Data members
        private List<PlayerShip> lives;
        private readonly int initialAmountOfLives;
        private const int AmountOfBufferShips = 1;
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

        private static int addLifeWhenThereAreNoLives(int amountOfLives)
        {
            if (amountOfLives < 1)
            {
                amountOfLives = 1;
            }
            return amountOfLives;
        }

        #endregion

        #region Methods
        private void addLives(int lives)
        {
            PlayerShip initialPlayerShip = new PlayerShip();
            this.lives.Add(initialPlayerShip);

            for (int i = 0; i < lives; i++)
            {
                PlayerShip aPlayerShip = new PlayerShip();
                this.lives.Add(aPlayerShip);
            }

            PlayerShip bufferPlayerShip = new PlayerShip();
            this.lives.Add(bufferPlayerShip);
        }

        private void removeLifeInUse()
        {
            if (this.lives.Any())
            {
                PlayerShip playerShipInUse = this.lives.First();
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

        #region Properties

        public int AmountOfLives => this.lives.Count - AmountOfBufferShips;
        public bool IsThereAnyLives => this.lives.Count > AmountOfBufferShips;

        #endregion


    }
}