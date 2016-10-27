using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Devices;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    class AllEnemyShips
    {
        private List<EnemyShip> enemies;

        public AllEnemyShips(int numberOfEachShip, int amountOfLevels)
        {
            int levelCap = ++amountOfLevels;
            this.enemies = new List<EnemyShip>();


            for (int level = 1; level < levelCap; level++)
            {
                for (int i = 0; i < numberOfEachShip; i++)
                {
                    EnemyShip enemy = new EnemyShip(level);
                    this.enemies.Add(enemy);
                }
            }
            
        }

    }
}
