using System.Collections.Generic;

namespace SpaceInvaders.Model
{
    internal class Fleet
    {
        #region Data members

        private readonly int shipsPerLevel;

        #endregion

        #region Constructors

        public Fleet(int levels)
        {
            this.AmountOfLevels = levels;
            this.shipsPerLevel = 4;
            this.FleetProperty = new List<List<EnemyShip>>();

            this.addShipsToFleet(levels);
            this.addAllEnemiesToGame();
        }

        #endregion

        #region Methods

        private void addShipsToFleet(int levels)
        {
            for (var i = 0; i < levels; i++)
            {
                var enemyRow = new List<EnemyShip>();
                this.FleetProperty.Add(enemyRow);
            }
        }

        private void addAllEnemiesToGame()
        {
            var level = 0;
            foreach (var listOfEnemies in this.FleetProperty)
            {
                level++;
                this.addEnemiesToEachLevelOfEnemies(level, listOfEnemies);
            }
        }

        private void addEnemiesToEachLevelOfEnemies(int level, List<EnemyShip> listOfEnemies)
        {
            var amountOfEnemiesForCurrentLevel = this.GetAmountOfShipForCurrentLevel(level);
            for (var i = 0; i < amountOfEnemiesForCurrentLevel; i++)
            {
                var ship = new EnemyShip(level);
                listOfEnemies.Add(ship);
            }
        }

        public void removeEnemyFromFleet(EnemyShip enemyShip)
        {
            foreach (var enemyRows in this.FleetProperty)
            {
                enemyRows.Remove(enemyShip);
            }
        }

        //Gets ships that can fire
        public
            List<EnemyShip> GetFiringShips()
        {
            var firingShips = new List<EnemyShip>();
            var levelThree = 3;

            foreach (var enemyRows in this.FleetProperty)
            {
                foreach (var enemy in enemyRows)
                {
                    if (enemy.GetLevel >= levelThree)
                    {
                        firingShips.Add(enemy);
                    }
                }
            }
            return firingShips;
        }

        public List<EnemyShip> GetEnemyShipsByLevel(int level)
        {
            var currentLevelShips = new List<EnemyShip>();

            foreach (var enemyRows in this.FleetProperty)
            {
                foreach (var enemy in enemyRows)
                {
                    if (enemy.GetLevel == level)
                    {
                        currentLevelShips.Add(enemy);
                    }
                }
            }
            return currentLevelShips;
        }

        public List<EnemyShip> GetAllEnemyShips()
        {
            var enemies = new List<EnemyShip>();

            foreach (var enemyRows in this.FleetProperty)
            {
                foreach (var enemy in enemyRows)
                {
                    enemies.Add(enemy);
                }
            }
            return enemies;
        }

        public int GetAmountOfShipForCurrentLevel(int level)
        {
            return level*2;
        }

        #endregion

        #region Property

        public int AmountOfLevels { get; }

        public List<List<EnemyShip>> FleetProperty { get; }

        #endregion
    }
}