﻿using System.Collections.Generic;
using SpaceInvaders.Util;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Fleet of enemy ships
    /// </summary>
    public class EnemyFleet
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyFleet" /> class base level 1.
        /// </summary>
        public EnemyFleet() : this(1)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyFleet" /> class.
        ///     Precondition: levels must be at least 1
        ///     Postcondition: EnemyFleet Data members are instantiated
        /// </summary>
        /// <param name="levels">The levels in this fleet.</param>
        public EnemyFleet(int levels)
        {
            if (levels < 0)
            {
                levels = 1;
            }
            Levels = levels;
            Fleet = new List<List<EnemyShip>>();

            addShipsToFleet(levels);
            addAllEnemiesToGame();
        }

        #endregion

        #region Methods

        private void addShipsToFleet(int levels)
        {
            for (var i = 0; i < levels; i++)
            {
                var enemyRow = new List<EnemyShip>();
                Fleet.Add(enemyRow);
            }
        }

        private void addAllEnemiesToGame()
        {
            var level = 0;
            foreach (var listOfEnemies in Fleet)
            {
                level++;
                addEnemiesToEachLevelOfEnemies(level, listOfEnemies);
            }
        }

        private void addEnemiesToEachLevelOfEnemies(int level, List<EnemyShip> listOfEnemies)
        {
            var amountOfEnemiesForCurrentLevel = GetAmountOfShipForLevel(level);
            for (var i = 0; i < amountOfEnemiesForCurrentLevel; i++)
            {
                if (level == (int) ShipFactory.ShipSelections.Level1EnemyShip)
                {
                    var ship = (EnemyShip) ShipFactory.SelectShip(ShipFactory.ShipSelections.Level1EnemyShip);
                    listOfEnemies.Add(ship);
                }
                else if (level == (int) ShipFactory.ShipSelections.Level2EnemyShip)
                {
                    var ship = (EnemyShip) ShipFactory.SelectShip(ShipFactory.ShipSelections.Level2EnemyShip);
                    listOfEnemies.Add(ship);
                }
                else if (level == (int) ShipFactory.ShipSelections.Level3EnemyShip)
                {
                    var ship = (EnemyShip) ShipFactory.SelectShip(ShipFactory.ShipSelections.Level3EnemyShip);
                    listOfEnemies.Add(ship);
                }
                else if (level >= (int) ShipFactory.ShipSelections.DefaultEnemyShip)
                {
                    var ship = (EnemyShip) ShipFactory.SelectShip(ShipFactory.ShipSelections.DefaultEnemyShip);
                    listOfEnemies.Add(ship);
                }
            }
        }

        /// <summary>
        ///     Removes the enemy from fleet.\
        ///     Precondition: enemyRows
        ///     Postcondition: Enemy is removed from one its list and the size of that list is decreased.
        /// </summary>
        /// <param name="enemyShip">The enemy ship to be removed.</param>
        public void RemoveEnemyFromFleet(EnemyShip enemyShip)
        {
            foreach (var enemyRow in Fleet)
            {
                if (enemyRow.Contains(enemyShip))
                {
                    enemyRow.Remove(enemyShip);
                }
                
            }
        }

        /// <summary>
        ///     Gets the firing ships.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <returns>List of ships that can fire</returns>
        public List<EnemyShip> GetFiringShips()
        {
            var firingShips = new List<EnemyShip>();

            foreach (var enemyRows in Fleet)
            {
                foreach (var enemy in enemyRows)
                {
                    if (enemy.Level >= 3)
                    {
                        firingShips.Add(enemy);
                    }
                }
            }
            return firingShips;
        }

        /// <summary>
        ///     Gets the enemy ships by level.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <param name="level">The level of ships to get.</param>
        /// <returns>List containing the enemies of the secified level</returns>
        public List<EnemyShip> GetEnemyShipsByLevel(int level)
        {
            var currentLevelShips = new List<EnemyShip>();

            foreach (var enemyRows in Fleet)
            {
                foreach (var enemy in enemyRows)
                {
                    if (enemy.Level == level)
                    {
                        currentLevelShips.Add(enemy);
                    }
                }
            }
            return currentLevelShips;
        }

        /// <summary>
        ///     Gets all enemy ships.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <returns>List of all ships in fleet as one list</returns>
        public List<EnemyShip> GetAllEnemyShips()
        {
            var enemies = new List<EnemyShip>();

            foreach (var enemyRows in Fleet)
            {
                foreach (var enemy in enemyRows)
                {
                    enemies.Add(enemy);
                }
            }
            return enemies;
        }

        /// <summary>
        ///     Gets the amount of ship for current level.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <param name="level">The level of the ships.</param>
        /// <returns>amount of ships in a row or level within the fleet</returns>
        public int GetAmountOfShipForLevel(int level)
        {
            var multiplier = 2;
            var shipsInLevel = multiplier*level;
            return shipsInLevel;
        }

        #endregion

        #region Property

        /// <summary>
        ///     Gets the amount of levels.
        /// </summary>
        /// <value>
        ///     The amount of levels.
        /// </value>
        public int Levels { get; }

        /// <summary>
        ///     Gets the entire fleet.
        /// </summary>
        /// <value>
        ///     The fleet.
        /// </value>
        public List<List<EnemyShip>> Fleet { get; }
        
        #endregion
    }
}