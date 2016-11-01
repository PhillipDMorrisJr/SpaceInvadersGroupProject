using SpaceInvaders.Model;

namespace SpaceInvaders.Util
{
    internal static class ShipFactory
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
                var enemyLevel = (int) shipSelectionsRequested;
                var ship = new EnemyShip(enemyLevel);
                return ship;
            }
            var aPlayer = new PlayerShip();
            return aPlayer;
        }
    }
}