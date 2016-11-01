using SpaceInvaders.Model;

namespace SpaceInvaders.Util
{
    static class ShipFactory
    {
        public enum ShipSelections
        {
            BonusShip,
            Level1EnemyShip,
            Level2EnemyShip,
            Level3EnemyShip,
            DefaultEnemyShip,
            PlayerShip
        }

        public static GameObject SelectShip(ShipSelections shipSelectionsRequested)
        {

            if ((int) shipSelectionsRequested < (int) ShipSelections.PlayerShip)
            {
                int enemyLevel = (int) shipSelectionsRequested;
                EnemyShip ship = new EnemyShip(enemyLevel);
                return ship;
            }
            else
            {
                PlayerShip aPlayer = new PlayerShip();
                return aPlayer;
            }





        }
    }
}
