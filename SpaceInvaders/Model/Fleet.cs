using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    class Fleet
    {
        private List<List<EnemyShip>> fleet;

        private readonly int shipsPerLevel;
        private readonly int amountOfLevels;

        public Fleet(int levels)
        {
            this.amountOfLevels = levels;
            this.shipsPerLevel = 4;
            this.fleet = new List<List<EnemyShip>>();

            this.addShipsToFleet(levels);
            this.addAllEnemiesToGame();
        }

        private void addShipsToFleet(int levels)
        {
            for (var i = 0; i < levels; i++)
            {
                List<EnemyShip> enemyRow = new List<EnemyShip>();
                this.fleet.Add(enemyRow);
            }
        }


        private void addAllEnemiesToGame()
        {
            var level = 0;
            foreach (var listOfEnemies in this.fleet)
            {
                level++;
                this.addEnemiesToEachLevelOfEnemies(level, listOfEnemies);
            }
        }

        private void addEnemiesToEachLevelOfEnemies(int level, List<EnemyShip> listOfEnemies)
        {
            int amountOfEnemiesForCurrentLevel = this.GetAmountOfShipForCurrentLevel(level);
            for (var i = 0; i < amountOfEnemiesForCurrentLevel; i++)
            {
                var ship = new EnemyShip(level);
                listOfEnemies.Add(ship);
            }
        }


        public void removeEnemyFromFleet(EnemyShip enemyShip)
        {
            foreach (var enemyRows in this.fleet)
            {
                enemyRows.Remove(enemyShip);
            }
        }



        //Gets ships that can fire
            public
            List<EnemyShip> GetFiringShips()
        {
            List<EnemyShip> firingShips = new List<EnemyShip>();
            int levelThree = 3;

            foreach (var enemyRows in this.fleet)
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
            List<EnemyShip> currentLevelShips = new List<EnemyShip>();

            foreach (var enemyRows in this.fleet)
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
            List<EnemyShip> enemies = new List<EnemyShip>();

            foreach (var enemyRows in this.fleet)
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




 #region Property
        public int AmountOfLevels => this.amountOfLevels;


        public List<List<EnemyShip>> FleetProperty
        {
            get { return this.fleet; }
        }

        #endregion
    } 
   
}