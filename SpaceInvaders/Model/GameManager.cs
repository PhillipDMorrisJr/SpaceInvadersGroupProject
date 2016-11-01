using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        #region Data members

        /// <summary>
        ///     The tick interval in milliseconds
        /// </summary>
        public const int TickInterval = 25;

        private const double PlayerShipBottomOffset = 30;
        private const double EnemyShipOffset = 10;
        private const double Half = 0.5;

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        private readonly TimeSpan gameTickInterval = new TimeSpan(0, 0, 0, 0, TickInterval);
        private DispatcherTimer gameTimer;
        private int currentLevel;
        private EnemyFleet fleet;

        private List<Bullet> playerAmmo;
        private List<Bullet> enemyAmmo;

        private PlayerShip playerShip;
        private PlayerShipFactory playerPlayerShipFactory;
        private Canvas currentBackground;

        private int enemyMotionCounter;
        private Scoreboard gameScoreboard;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new default instance of the <see cref="GameManager" /> class.
        /// </summary>
        public GameManager() : this(500, 500)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Precondition: backgroundHeight > 0 AND backgroundWidth > 0
        /// </summary>
        /// <param name="backgroundHeight">The backgroundHeight of the game play window.</param>
        /// <param name="backgroundWidth">The backgroundWidth of the game play window.</param>
        public GameManager(double backgroundHeight, double backgroundWidth)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            currentLevel = 1;
            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;


            initializePlayerShipFactory();

            fleet = new EnemyFleet(4);

            initializeDataMembers();

            enemyMotionCounter = 0;

            initializeTimer();
        }

        private void initializeDataMembers()
        {
            playerAmmo = new List<Bullet>();
            enemyAmmo = new List<Bullet>();

            gameScoreboard = new Scoreboard();
        }

        private void initializePlayerShipFactory()
        {
            playerPlayerShipFactory = new PlayerShipFactory(3);
            playerShip = playerPlayerShipFactory.UseLife();
        }

        #endregion

        #region Methods

        private void initializeTimer()
        {
            this.gameTimer = new DispatcherTimer {Interval = this.gameTickInterval};

            this.gameTimer.Tick += this.gameTimerOnTick;
            this.gameTimer.Start();
        }

        /// <summary>
        ///     Initializes the game placing player ship and ship ship in the game.
        ///     Precondition: background != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="background">The background canvas.</param>
        public void InitializeGame(Canvas background)
        {
            if (background == null)
            {
                throw new ArgumentNullException(nameof(background));
            }
            currentBackground = background;

            addPlayerShipToGame();
            addEnemyShipsToGame();
        }

        private void gameTimerOnTick(object sender, object e)
        {
            MoveEnemyShips();
            FireEnemyBullet();
            HandleBullets();
            stopTimerAtGameOver();
        }

        /// <summary>
        ///     Manages player bullet when fired
        ///     Precondition: There must be bullets in player ammo
        ///     Postcondition: Bullet is fired
        /// </summary>
        public void FirePlayerBullet()
        {
            var isPlayerWaiting = checkIfPlayerIsWaitingToFire();
            checkIfPlayerAmmoShouldBeStocked(isPlayerWaiting);
            handlePlayerBulletHits();
        }

        private bool checkIfPlayerIsWaitingToFire()
        {
            var isBulletWaiting = false;
            if (playerAmmo.Any())
            {
                var bulletClosetToShip = playerAmmo.Max(bullet => bullet.Y + bullet.Height);

                isBulletWaiting = bulletClosetToShip > playerShip.Y - playerShip.Height;
            }
            return isBulletWaiting;
        }

        private void checkIfPlayerAmmoShouldBeStocked(bool isBulletWaiting)
        {
            if (!playerAmmo.Any() || !isBulletWaiting)
            {
                stockPlayerAmmo();
            }
        }

        /// <summary>
        ///     Manages enemy bullet when fired
        ///     Precondition: the enemy must have bullets
        ///     Postcondition: Enemy fires bullet
        /// </summary>
        public void FireEnemyBullet()
        {
            handleEachEnemyBullet();
            var enemyBulletsToRemove = new List<Bullet>(enemyAmmo);
            foreach (var enemyBullet in enemyBulletsToRemove)
            {
                moveEnemyBulletDownWhenBulletIsInCanvas(enemyBullet);
                removeEnemyBulletFromPlayWhenItHitsPlayerShip(enemyBullet);
            }
        }

        private void removeEnemyBulletFromPlayWhenItHitsPlayerShip(Bullet enemyBullet)
        {
            if (bulletHitShip(enemyBullet, playerShip))
            {
                currentBackground.Children.Remove(playerShip.Sprite);
                removeBulletFromGame(enemyBullet);

                bringPlayerBackToLifeWhenThereAreMoreLives();
            }
        }

        private void bringPlayerBackToLifeWhenThereAreMoreLives()
        {
            gameTimer.Stop();
            playerShip.Destroyed = true;

            if (playerPlayerShipFactory.IsThereAnyLives)
            {
                if (playerShip.Destroyed)
                {
                    playerShip = playerPlayerShipFactory.UseLife();
                    addPlayerShipToGame();
                    gameTimer.Start();
                }
            }
        }


        private void moveEnemyBulletDownWhenBulletIsInCanvas(Bullet enemyBullet)
        {
            if (currentBackground.Children.Contains(enemyBullet.Sprite))
            {
                enemyBullet.MoveDown();
            }
        }

        private void stockPlayerAmmo()
        {
            if (playerAmmo.Count < this.BulletLimit)
            {
                var aBullet = new Bullet();
                playerAmmo.Add(aBullet);
                currentBackground.Children.Add(aBullet.Sprite);
                placePlayerBullet(aBullet);
            }
        }

        private void handleEachEnemyBullet()
        {
            fireBulletAtPlayer();
            deleteBulletsBeyondBoundary();
        }

        private void fireBulletAtPlayer()
        {
            var randomizer = new Random();
            var firingShips = fleet.GetFiringShips();

            fireFiringShips(firingShips, randomizer);
        }

        private void fireFiringShips(List<EnemyShip> firingShips, Random randomizer)
        {
            if (firingShips.Any())
            {
                fireMultipleShips(randomizer, firingShips);
            }
        }

        private void fireMultipleShips(Random randomizer, List<EnemyShip> firingShips)
        {
            var amountOfShipsToFire = randomizer.Next(firingShips.Count());
            for (var i = 0; i < amountOfShipsToFire; i++)
            {
                var firingShip = selectFiringShip(randomizer, amountOfShipsToFire, firingShips);
                fireShipWhenShipHasNotFired(firingShip);
            }
        }

        private void fireShipWhenShipHasNotFired(EnemyShip firingShip)
        {
            var isEnemyWaiting = checkIfEnemyIsWaiting(firingShip);
            if (!enemyAmmo.Any() || !isEnemyWaiting)
            {
                addEnemyBulletsToScreen(firingShip);
                firingShip.HasFired = true;
            }
        }

        private bool checkIfEnemyIsWaiting(EnemyShip enemy)
        {
            var isEnemyWaiting = false;

            if (enemyAmmo.Any())
            {
                var bulletClosetToShip = enemyAmmo.Max(bullet => bullet.Y + bullet.Height);

                isEnemyWaiting = bulletClosetToShip < enemy.Y - enemy.Height;
            }
            return isEnemyWaiting;
        }

        private EnemyShip selectFiringShip(Random randomizer, int amountOfShipsToFire,
            List<EnemyShip> firingShips)
        {
            var firingShipIndex = randomizer.Next(amountOfShipsToFire);
            var firingShip = firingShips[firingShipIndex];
            return firingShip;
        }

        private void deleteBulletsBeyondBoundary()
        {
            for (var enemyBulletIterator = 0; enemyBulletIterator < enemyAmmo.Count; enemyBulletIterator++)
            {
                removeAllEnemyBulletsBeyondBoundary(enemyBulletIterator);
            }
        }

        private void removeAllEnemyBulletsBeyondBoundary(int i)
        {
            if (enemyAmmo[i].Y >= backgroundHeight)
            {
                currentBackground.Children.Remove(enemyAmmo[i].Sprite);
                enemyAmmo.RemoveAt(i);
            }
        }

        private void addEnemyBulletsToScreen(EnemyShip enemy)
        {
            var randomBullet = new Bullet();
            var maxAmmo = this.BulletLimit;

            if (enemyAmmo.Count < maxAmmo)
            {
                enemyAmmo.Add(randomBullet);
                currentBackground.Children.Add(randomBullet.Sprite);
                placeEnemyBullet(enemy, randomBullet);
            }
        }

        private void placeEnemyBullet(EnemyShip enemy, Bullet bullet)
        {
            bullet.X = enemy.X + enemy.Sprite.ActualWidth*Half + bullet.X*Half;
            bullet.Y = enemy.Y;
        }

        /// <summary>
        ///     Manages player bullets
        ///     Precondition: none
        ///     Postcondition: Bullets beyond boundaries and bullets that have collided with other Game objects are removedfrom
        ///     game.
        /// </summary>
        public void HandleBullets()
        {
            handlePlayerBullets();
            handlePlayerBulletHits();
        }

        private void handlePlayerBullets()
        {
            var playerBulletsToRemove = new List<Bullet>(playerAmmo);
            foreach (var bullet in playerBulletsToRemove)
            {
                moveBulletUpUntilOutOfBounds(bullet);
            }
        }

        private void moveBulletUpUntilOutOfBounds(Bullet bullet)
        {
            var backgroundOrigin = 0;

            if (bullet.Y > backgroundOrigin)
            {
                bullet.MoveUp();
            }
            else
            {
                removeBulletFromGame(bullet);
            }
        }

        private void removeBulletFromGame(Bullet bullet)
        {
            currentBackground.Children.Remove(bullet.Sprite);
            if (playerAmmo.Contains(bullet))
            {
                playerAmmo.Remove(bullet);
            }
            else if (enemyAmmo.Contains(bullet))
            {
                enemyAmmo.Remove(bullet);
            }
        }

        private void addEnemyShipsToGame()
        {
            addEnemyToEachRowOfEnemies();

            placeEnemyShipsNearTopOfBackgroundCentered();
        }

        private void addEnemyToEachRowOfEnemies()
        {
            foreach (var enemy in fleet.GetAllEnemyShips())
            {
                currentBackground.Children.Add(enemy.Sprite);
            }
        }

        private void addPlayerShipToGame()
        {
            currentBackground.Children.Add(playerShip.Sprite);

            placePlayerShipNearBottomOfBackgroundCentered();
        }

        private void placeEnemyShipsNearTopOfBackgroundCentered()
        {
            for (var levelIterator = 1; levelIterator <= fleet.Levels; levelIterator++)
            {
                var enemyRowCount = fleet.GetAmountOfShipForLevel(levelIterator);
                var enemyXOrigin = calculateEnemyXOrigin(enemyRowCount);

                setXLocationForShipsAfterFirstShip(levelIterator, enemyXOrigin);
            }
        }

        private double calculateEnemyXOrigin(int enemyRowCount)
        {
            var levelOneShip = fleet.GetAllEnemyShips().First();
            return backgroundWidth*Half - levelOneShip.Width*(enemyRowCount*Half) -
                   EnemyShipOffset;
        }

        private void setXLocationForShipsAfterFirstShip(int level, double enemyXOrigin)
        {
            if (level > 1)
            {
                var previousLevel = level - 1;
                var previousRow = fleet.GetEnemyShipsByLevel(previousLevel);
                var firstShipOfPreviousRow = previousRow.First();

                enemyXOrigin = calculateNextShipXOrigin(firstShipOfPreviousRow);
            }
            placeEnemiesInOrder(enemyXOrigin, level);
        }

        private double calculateNextShipXOrigin(EnemyShip firstShipOfPreviousRow)
        {
            return firstShipOfPreviousRow.X - (firstShipOfPreviousRow.Width +
                                               EnemyShipOffset);
        }

        private void placeEnemiesInOrder(double enemyXLocation, int level)
        {
            var amountOfShipsForCurrentLevel = fleet.GetAmountOfShipForLevel(level);
            var enemyRow = fleet.GetEnemyShipsByLevel(level);

            for (var shipIterator = 0; shipIterator < amountOfShipsForCurrentLevel; shipIterator++)
            {
                var currentEnemy = enemyRow[shipIterator];

                currentEnemy.X = enemyXLocation;

                enemyXLocation = enemyXLocation + currentEnemy.Width +
                                 EnemyShipOffset;

                setEnemyYLocation(level, currentEnemy);
            }
        }

        private void setEnemyYLocation(int level, EnemyShip currentEnemy)
        {
            if (level == 1)
            {
                currentEnemy.Y = backgroundHeight*Half;
            }
            else
            {
                var previousLevel = level - 1;
                var prevEnemyRow = fleet.GetEnemyShipsByLevel(previousLevel);
                var higherLevelEnemy = prevEnemyRow.First();
                currentEnemy.Y = higherLevelEnemy.Y - (EnemyShipOffset + currentEnemy.Height);
            }
        }

        private void placePlayerShipNearBottomOfBackgroundCentered()
        {
            playerShip.X = (backgroundWidth - playerShip.Width)*Half;
            playerShip.Y = backgroundHeight - playerShip.Height - PlayerShipBottomOffset;
        }

        /// <summary>
        ///     Moves the player ship to the left.
        ///     Precondition: The player ship  must be to the right of the origin
        ///     Postcondition: The player ship has moved left.
        /// </summary>
        public void MovePlayerShipLeft()
        {
            var backgroundOrigin = playerShip.SpeedX;
            if (isPlayerIsRightOfOrigin(backgroundOrigin))
            {
                playerShip.MoveLeft();
            }
        }

        private bool isPlayerIsRightOfOrigin(int backgroundOrigin)
        {
            return playerShip.X > backgroundOrigin;
        }

        /// <summary>
        ///     Moves the player ship to the right.
        ///     Precondition: The player ship  must be to the left of the right side boundary
        ///     Postcondition: The player ship has moved right.
        /// </summary>
        public void MovePlayerShipRight()
        {
            var backgroundRightSideBoundary = backgroundWidth - (playerShip.Width + playerShip.SpeedX);

            if (isPlayerIsLeftOfRightSideBoundary(backgroundRightSideBoundary))
            {
                playerShip.MoveRight();
            }
        }

        private bool isPlayerIsLeftOfRightSideBoundary(double backgroundRightSideBoundary)
        {
            return playerShip.X < backgroundRightSideBoundary;
        }

        private void moveAllEnemyShipsRight()
        {
            foreach (var enemy in fleet.GetAllEnemyShips())
            {
                moveEachEnemyInFleetRight(enemy);
            }
        }

        private void moveEachEnemyInFleetRight(EnemyShip enemy)
        {
            if (enemy.X < backgroundWidth)
            {
                enemy.MoveRight();
            }
        }

        private void moveAllEnemyShipsLeft()
        {
            foreach (var enemy in fleet.GetAllEnemyShips())
            {
                moveEachInFleetEnemyLeft(enemy);
            }
        }

        private void moveEachInFleetEnemyLeft(EnemyShip enemy)
        {
            if (enemy.X < backgroundWidth)
            {
                enemy.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves enemy ships right to left then left to right
        ///     Precondition: Enemies are created
        ///     Postcondition: Enemies continue to move until the game ends
        /// </summary>
        public void MoveEnemyShips()
        {
            var interval = 20;
            var firstIntervalUpperBound = 10;

            var secondIntervalUpperBound = firstIntervalUpperBound + interval;
            var thirdIntervalUpperBound = secondIntervalUpperBound + interval;

            enemyMotionCounter++;
            checkCounters(interval, firstIntervalUpperBound, thirdIntervalUpperBound);
            setEnemyCounterToFirstIntervalAfterLastInterval(thirdIntervalUpperBound, firstIntervalUpperBound);
        }

        private void checkCounters(int interval, int firstIntervalUpperBound, int thirdIntervalUpperBound)
        {
            var secondIntervalLowerBound = firstIntervalUpperBound;
            var secondIntervalUpperBound = firstIntervalUpperBound + interval;
            var thirdIntervalLowerBound = secondIntervalUpperBound;

            checkIfEnemyCounterInFirstInterval(firstIntervalUpperBound);
            checkIfEnemyCounterInSecondInterval(secondIntervalLowerBound, secondIntervalUpperBound);
            checkIfEnemyCounteBetweenThirdInterval(thirdIntervalLowerBound, thirdIntervalUpperBound);
        }

        private void setEnemyCounterToFirstIntervalAfterLastInterval(int thirdIntervalUpperBound,
            int firstIntervalUpperBound)
        {
            if (enemyMotionCounter > thirdIntervalUpperBound)
            {
                enemyMotionCounter = firstIntervalUpperBound;
            }
        }

        private void checkIfEnemyCounteBetweenThirdInterval(int secondIntervalLowerBound, int secondIntervalUpperBound)
        {
            if ((enemyMotionCounter > secondIntervalLowerBound) &&
                (enemyMotionCounter <= secondIntervalUpperBound))
            {
                moveAllEnemyShipsRight();
            }
        }

        private void checkIfEnemyCounterInSecondInterval(int secondIntervalLowerBound, int secondIntervalUpperBound)
        {
            if ((enemyMotionCounter > secondIntervalLowerBound) && (enemyMotionCounter <= secondIntervalUpperBound))
            {
                moveAllEnemyShipsLeft();
            }
        }

        private void checkIfEnemyCounterInFirstInterval(int firstIntervalUpperBound)
        {
            if (enemyMotionCounter <= firstIntervalUpperBound)
            {
                moveAllEnemyShipsRight();
            }
        }

        private void handlePlayerBulletHits()
        {
            var currentfleet = fleet.GetAllEnemyShips();
            foreach (var enemy in currentfleet)
            {
                killEnemiesHit(enemy);
            }
        }

        private void placePlayerBullet(Bullet bullet)
        {
            var halfOfShip = playerShip.Sprite.Width*Half;
            bullet.X = playerShip.X + halfOfShip - bullet.Sprite.Width*Half;
            bullet.Y = playerShip.Y;
        }

        private void killEnemiesHit(EnemyShip enemy)
        {
            if (playerAmmo.Any())
            {
                removeEachEnemyHitByBulletFromGame(enemy);
            }
        }

        private void removeEachEnemyHitByBulletFromGame(EnemyShip enemy)
        {
            var playerBulletsFired = new List<Bullet>(playerAmmo);
            foreach (var bulletFired in playerBulletsFired)
            {
                removeEnemyHitByBulletFromGame(bulletFired, enemy);
            }
        }

        private void removeEnemyHitByBulletFromGame(Bullet bullet, EnemyShip enemy)
        {
            if (bulletHitShip(bullet, enemy))
            {
                gameScoreboard.IncreaseScore(enemy.Level);
                fleet.RemoveEnemyFromFleet(enemy);
                removeBulletFromGame(bullet);
                currentBackground.Children.Remove(enemy.Sprite);
            }
        }

        private bool bulletHitShip(Bullet bullet, GameObject ship)
        {
            var rightSideOfEnemy = ship.X + ship.Width;
            var enemyXOrigin = ship.X;

            var bottomOfEnemy = ship.Y + ship.Height;
            var enemyYOrigin = ship.Y;

            var bulletInXBound = (enemyXOrigin <= bullet.X) && (bullet.X <= rightSideOfEnemy);
            var bulletInYBound = (enemyYOrigin <= bullet.Y) && (bullet.Y <= bottomOfEnemy);

            if (bulletInXBound && bulletInYBound)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if the game is over
        ///     Precondition: none
        ///     PostCondition: true is returned if the game has ended
        /// </summary>
        /// <returns>return true when the game is over</returns>
        public bool IsGameOver()
        {
            if (!playerPlayerShipFactory.IsThereAnyLives || areEnemyShipsOutOfPlay())
            {
                if (currentLevel < 3 && playerPlayerShipFactory.IsThereAnyLives)
                {
                    currentLevel++;
                    gameTimer.Stop();
                    goToNextRound();
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool areEnemyShipsOutOfPlay()
        {
            if (
                !(fleet.GetAllEnemyShips().Any() &&
                  currentBackground.Children.Contains(playerShip.Sprite)))
            {
                return true;
            }
            return false;
        }

        private void stopTimerAtGameOver()
        {
            if (IsGameOver())
            {
                resetCanvas();
                gameTimer.Stop();
            }
        }

        private void goToNextRound()
        {
            resetCanvas();

            initializePlayerShipFactory();

            fleet = new EnemyFleet(fleet.Levels + 1);

            initializeDataMembers();

            enemyMotionCounter = 0;

            initializeTimer();
            InitializeGame(currentBackground);
        }

        private string displayGameStatistics()
        {
            return "Score: " + gameScoreboard.Score + "\n" + "Lives: " + playerPlayerShipFactory.AmountOfLives;
        }

        private void resetCanvas()
        {
            currentBackground.Children.Remove(playerShip.Sprite);
            clearEnemies();

            clearBullets();
        }

        private void clearBullets()
        {
            clearEnemyBullets();
            clearPlayerBullets();
        }

        private void clearPlayerBullets()
        {
            var playerBulletsToRemove = new List<Bullet>(playerAmmo);
            foreach (var bullet in playerBulletsToRemove)
            {
                removeBulletFromGame(bullet);
            }
        }

        private void clearEnemyBullets()
        {
            var enemyBulletsToRemove = new List<Bullet>(enemyAmmo);
            foreach (var bullet in enemyBulletsToRemove)
            {
                removeBulletFromGame(bullet);
            }
        }

        private void clearEnemies()
        {
            var enemiesToRemove = new List<EnemyShip>(fleet.GetAllEnemyShips());

            foreach (var enemy in enemiesToRemove)
            {
                fleet.RemoveEnemyFromFleet(enemy);
                currentBackground.Children.Remove(enemy.Sprite);
            }
        }

        #endregion

        #region Property

        /// <summary>
        ///     String GameStatistics records game score and player lives
        /// </summary>
        public string GameStatistics => displayGameStatistics();

        /// <summary>
        ///     Records game score
        /// </summary>
        public int GameScore => gameScoreboard.Score;

        private int BulletLimit { get; } = 3;

        #endregion
    }
}