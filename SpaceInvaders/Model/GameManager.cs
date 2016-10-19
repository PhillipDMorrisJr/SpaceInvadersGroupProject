using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private const double PlayerShipTopOffset = 30;
        private const double EnemyShipOffset = 10;
        private const double Half = 0.5;

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        private readonly TimeSpan gameTickInterval = new TimeSpan(0, 0, 0, 0, TickInterval);
        private DispatcherTimer gameTimer;

        private readonly EnemyFleet fleet;

        private readonly List<Bullet> playerAmmo;
        private readonly List<Bullet> enemyAmmo;

        private PlayerShip playerShip;
        private readonly PlayerShipLives playerPlayerShipLives;
        private Canvas currentBackground;

        private int enemyMotionCounter;
        private readonly Scoreboard gameScoreboard;

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

            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;

            this.initializeTimer();

            this.playerPlayerShipLives = new PlayerShipLives(3);
            this.playerShip = this.playerPlayerShipLives.UseLife();

            this.fleet = new EnemyFleet(4);

            this.playerAmmo = new List<Bullet>();
            this.enemyAmmo = new List<Bullet>();

            this.gameScoreboard = new Scoreboard();

            this.enemyMotionCounter = 0;
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
            this.currentBackground = background;

            this.addPlayerShipToGame();
            this.addEnemyShipsToGame();
        }

        private void gameTimerOnTick(object sender, object e)
        {
            this.MoveEnemyShips();
            this.FireEnemyBullet();
            this.HandleBullets();
            this.stopTimerAtGameOver();
        }

        /// <summary>
        ///     Manages player bullet when fired
        ///     Precondition: There must be bullets in player ammo
        ///     Postcondition: Bullet is fired
        /// </summary>
        public void FirePlayerBullet()
        {
            var isPlayerWaiting = false;
            isPlayerWaiting = this.checkIfPlayerIsWaitingToFire(isPlayerWaiting);
            this.checkIfPlayerAmmoShouldBeStocked(isPlayerWaiting);
            this.handlePlayerBulletHits();
        }

        private bool checkIfPlayerIsWaitingToFire(bool isBulletWaiting)
        {
            if (this.playerAmmo.Any())
            {
                var bulletClosetToShip = this.playerAmmo.Max(bullet => bullet.Y + bullet.Height);

                isBulletWaiting = bulletClosetToShip > this.playerShip.Y - this.playerShip.Height;
            }
            return isBulletWaiting;
        }

        private void checkIfPlayerAmmoShouldBeStocked(bool isBulletWaiting)
        {
            if (!this.playerAmmo.Any() || !isBulletWaiting)
            {
                this.stockPlayerAmmo();
            }
        }

        /// <summary>
        ///     Manages enemy bullet when fired
        ///     Precondition: the enemy must have bullets
        ///     Postcondition: Enemy fires bullet
        /// </summary>
        public void FireEnemyBullet()
        {
            this.handleEachEnemyBullet();

            for (var i = 0; i < this.enemyAmmo.Count; i++)
            {
                this.moveEnemyBulletDownWhenBulletIsInCanvas(this.enemyAmmo[i]);
                this.removeEnemyBulletFromPlayWhenItHitsPlayerShip(this.enemyAmmo[i]);
            }
        }

        private void removeEnemyBulletFromPlayWhenItHitsPlayerShip(Bullet enemyBullet)
        {
            if (this.bulletHitShip(enemyBullet, this.playerShip))
            {
                this.currentBackground.Children.Remove(this.playerShip.Sprite);
                this.removeEnemyBullet(enemyBullet);

                this.bringPlayerBackToLifeWhenThereAreMoreLives();
            }
        }

        private void bringPlayerBackToLifeWhenThereAreMoreLives()
        {
            this.gameTimer.Stop();
            this.playerShip.Destroyed = true;

            if (this.playerPlayerShipLives.IsThereAnyLives)
            {
                if (this.playerShip.Destroyed)
                {
                    this.playerShip = this.playerPlayerShipLives.UseLife();
                    this.addPlayerShipToGame();
                    this.gameTimer.Start();
                }
            }
        }

        private void removeEnemyBullet(Bullet enemyBullet)
        {
            this.currentBackground.Children.Remove(enemyBullet.Sprite);
            this.enemyAmmo.Remove(enemyBullet);
        }

        private void moveEnemyBulletDownWhenBulletIsInCanvas(Bullet enemyBullet)
        {
            if (this.currentBackground.Children.Contains(enemyBullet.Sprite))
            {
                enemyBullet.MoveDown();
            }
        }

        private void stockPlayerAmmo()
        {
            if (this.playerAmmo.Count < 3)
            {
                var aBullet = new Bullet();
                this.playerAmmo.Add(aBullet);
                this.currentBackground.Children.Add(aBullet.Sprite);
                this.placePlayerBullet(aBullet);
            }
        }

        private void handleEachEnemyBullet()
        {
            this.fireBulletAtPlayer();
            this.deleteBulletsBeyondBoundary();
        }

        private void fireBulletAtPlayer()
        {
            var randomizer = new Random();
            var firingShips = this.fleet.GetFiringShips();

            this.fireFiringShips(firingShips, randomizer);
        }

        private void fireFiringShips(List<EnemyShip> firingShips, Random randomizer)
        {
            if (firingShips.Any())
            {
                this.fireMultipleShips(randomizer, firingShips);
            }
        }
        
        private void fireMultipleShips(Random randomizer, List<EnemyShip> firingShips)
        {
            var amountOfShipsToFire = randomizer.Next(firingShips.Count());
            for (var i = 0; i < amountOfShipsToFire; i++)
            {
                var firingShip = selectFiringShip(randomizer, amountOfShipsToFire, firingShips);
                this.fireShipWhenShipHasNotFired(firingShip);
            }
        }

        private void fireShipWhenShipHasNotFired(EnemyShip firingShip)
        {
            bool isEnemyWaiting = false;

            isEnemyWaiting = this.checkIfEnemyIsWaiting(isEnemyWaiting, firingShip);
            if (!this.enemyAmmo.Any() || !isEnemyWaiting)
            {
                this.addEnemyBulletsToScreen(firingShip);
                firingShip.HasFired = true;
            }
        }

private bool checkIfEnemyIsWaiting(bool isEnemyWaiting, EnemyShip enemy)
        {
            if (this.enemyAmmo.Any())
            {
                var bulletClosetToShip = this.enemyAmmo.Max(bullet => bullet.Y + bullet.Height);

                isEnemyWaiting = bulletClosetToShip < enemy.Y - enemy.Height;
            }
            return isEnemyWaiting;
        }
        private static EnemyShip selectFiringShip(Random randomizer, int amountOfShipsToFire,
            List<EnemyShip> firingShips)
        {
            var firingShipIndex = randomizer.Next(amountOfShipsToFire);
            var firingShip = firingShips[firingShipIndex];
            return firingShip;
        }

        private void deleteBulletsBeyondBoundary()
        {
            for (var enemyBulletIterator = 0; enemyBulletIterator < this.enemyAmmo.Count; enemyBulletIterator++)
            {
                this.removeAllEnemyBulletsBeyondBoundary(enemyBulletIterator);
            }
        }

        private void removeAllEnemyBulletsBeyondBoundary(int i)
        {
            if (this.enemyAmmo[i].Y >= this.backgroundHeight)
            {
                this.currentBackground.Children.Remove(this.enemyAmmo[i].Sprite);
                this.enemyAmmo.RemoveAt(i);
            }
        }

        private void addEnemyBulletsToScreen(EnemyShip enemy)
        {
            var randomBullet = new Bullet();
            var maxAmmo = 3;

            if (this.enemyAmmo.Count < maxAmmo)
            {
                this.enemyAmmo.Add(randomBullet);
                this.currentBackground.Children.Add(randomBullet.Sprite);
                this.placeEnemyBullet(enemy, randomBullet);
            }
        }

        private void placeEnemyBullet(EnemyShip enemy, Bullet randomBullet)
        {
            
            randomBullet.X = enemy.X + enemy.Sprite.ActualWidth/2;
            randomBullet.Y = enemy.Y;
        }

        /// <summary>
        ///     Manages player bullets
        ///     Precondition: none
        ///     Postcondition: Bullets beyond boundaries and bullets that have collided with other Game objects are removedfrom
        ///     game.
        /// </summary>
        public void HandleBullets()
        {
            this.handlePlayerBullets();
            this.handlePlayerBulletHits();
        }

        private void handlePlayerBullets()
        {
            for (var i = 0; i < this.playerAmmo.Count; i++)
            {
                var bullet = this.playerAmmo[i];
                this.moveBulletUpUntilOutOfBounds(bullet);
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
                this.removePlayerBullet(bullet);
            }
        }

        private void removePlayerBullet(Bullet bullet)
        {
            this.currentBackground.Children.Remove(bullet.Sprite);
            this.playerAmmo.Remove(bullet);
        }

        private void addEnemyShipsToGame()
        {
            this.addEnemyToEachRowOfEnemies();

            this.placeEnemyShipsNearTopOfBackgroundCentered();
        }

        private void addEnemyToEachRowOfEnemies()
        {
            foreach (var enemy in this.fleet.GetAllEnemyShips())
            {
                this.currentBackground.Children.Add(enemy.Sprite);
            }
        }

        private void addPlayerShipToGame()
        {
            this.currentBackground.Children.Add(this.playerShip.Sprite);

            this.placePlayerShipNearBottomOfBackgroundCentered();
        }

        private void placeEnemyShipsNearTopOfBackgroundCentered()
        {
            for (var levelIterator = 1; levelIterator <= this.fleet.AmountOfLevels; levelIterator++)
            {
                var levelOneShip = this.fleet.GetAllEnemyShips().First();
                var enemyRowCount = this.fleet.GetAmountOfShipForLevel(levelIterator);
                var enemyXOrigin = this.calculateEnemyXOrigin(levelOneShip, enemyRowCount);

                this.setXLocationForShipsAfterFirstShip(levelIterator, enemyXOrigin);
            }
        }

        private double calculateEnemyXOrigin(EnemyShip levelOneShip, int enemyRowCount)
        {
            return this.backgroundWidth* Half - levelOneShip.Width*(enemyRowCount* Half) -
                   EnemyShipOffset;
        }

        private void setXLocationForShipsAfterFirstShip(int level, double enemyXOrigin)
        {
            if (level > 1)
            {
                var previousLevel = level - 1;
                var previousRow = this.fleet.GetEnemyShipsByLevel(previousLevel);
                var firstShipOfPreviousRow = previousRow.First();

                enemyXOrigin = this.calculateNextShipXOrigin(firstShipOfPreviousRow);
            }
            this.placeEnemiesInOrder(enemyXOrigin, level);
        }

        private double calculateNextShipXOrigin(EnemyShip firstShipOfPreviousRow)
        {
            return firstShipOfPreviousRow.X - (firstShipOfPreviousRow.Width +
                                               EnemyShipOffset);
        }

        private void placeEnemiesInOrder(double enemyXLocation, int level)
        {
            var amountOfShipsForCurrentLevel = this.fleet.GetAmountOfShipForLevel(level);
            var enemyRow = this.fleet.GetEnemyShipsByLevel(level);

            for (var shipIterator = 0; shipIterator < amountOfShipsForCurrentLevel; shipIterator++)
            {
                var currentEnemy = enemyRow[shipIterator];

                currentEnemy.X = enemyXLocation;

                enemyXLocation = enemyXLocation + currentEnemy.Width +
                                 EnemyShipOffset;

                this.setEnemyYLocation(level, currentEnemy);
            }
        }

        private void setEnemyYLocation(int level, EnemyShip currentEnemy)
        {
            var topCanvasBuffer = 20;
            if (level == 1)
            {
                currentEnemy.Y = (topCanvasBuffer + currentEnemy.Height)*this.fleet.AmountOfLevels;
            }
            else
            {
                var previousLevel = level - 1;
                var prevEnemyRow = this.fleet.GetEnemyShipsByLevel(previousLevel);
                var higherLevelEnemy = prevEnemyRow.First();
                currentEnemy.Y = higherLevelEnemy.Y - (EnemyShipOffset + currentEnemy.Height);
            }
        }

        private void placePlayerShipNearBottomOfBackgroundCentered()
        {
            this.playerShip.X = (this.backgroundWidth - this.playerShip.Width)* Half;
            this.playerShip.Y = this.backgroundHeight - this.playerShip.Height - PlayerShipBottomOffset;
        }

        /// <summary>
        ///     Moves the player ship to the left.
        ///     Precondition: The player ship  must be to the right of the origin
        ///     Postcondition: The player ship has moved left.
        /// </summary>
        public void MovePlayerShipLeft()
        {
            var backgroundOrigin = this.playerShip.SpeedX;
            if (this.isPlayerIsRightOfOrigin(backgroundOrigin))
            {
                this.playerShip.MoveLeft();
            }
        }

        private bool isPlayerIsRightOfOrigin(int backgroundOrigin)
        {
            return this.playerShip.X > backgroundOrigin;
        }

        /// <summary>
        ///     Moves the player ship to the right.
        ///     Precondition: The player ship  must be to the left of the right side boundary
        ///     Postcondition: The player ship has moved right.
        /// </summary>
        public void MovePlayerShipRight()
        {
            var backgroundRightSideBoundary = this.backgroundWidth - (this.playerShip.Width + this.playerShip.SpeedX);

            if (this.isPlayerIsLeftOfRightSideBoundary(backgroundRightSideBoundary))
            {
                this.playerShip.MoveRight();
            }
        }

        private bool isPlayerIsLeftOfRightSideBoundary(double backgroundRightSideBoundary)
        {
            return this.playerShip.X < backgroundRightSideBoundary;
        }

        private void moveAllEnemyShipsRight()
        {
            foreach (var enemy in this.fleet.GetAllEnemyShips())
            {
                this.moveEachEnemyInFleetRight(enemy);
            }
        }

        private void moveEachEnemyInFleetRight(EnemyShip enemy)
        {
            if (enemy.X < this.backgroundWidth)
            {
                enemy.MoveRight();
            }
        }

        private void moveAllEnemyShipsLeft()
        {
            foreach (var enemy in this.fleet.GetAllEnemyShips())
            {
                this.moveEachInFleetEnemyLeft(enemy);
            }
        }

        private void moveEachInFleetEnemyLeft(EnemyShip enemy)
        {
            if (enemy.X < this.backgroundWidth)
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

            var secondIntervalLowerBound = firstIntervalUpperBound;
            var secondIntervalUpperBound = firstIntervalUpperBound + interval;
            var thirdIntervalLowerBound = secondIntervalUpperBound;
            var thirdIntervalUpperBound = secondIntervalUpperBound + interval;

            this.enemyMotionCounter++;
            this.checkCounters(interval, firstIntervalUpperBound, thirdIntervalUpperBound);
            this.setEnemyCounterToFirstIntervalAfterLastInterval(thirdIntervalUpperBound, firstIntervalUpperBound);
        }

        private void checkCounters(int interval, int firstIntervalUpperBound, int thirdIntervalUpperBound)
        {
            var secondIntervalLowerBound = firstIntervalUpperBound;
            var secondIntervalUpperBound = firstIntervalUpperBound + interval;
            var thirdIntervalLowerBound = secondIntervalUpperBound;

            this.checkIfEnemyCounterInFirstInterval(firstIntervalUpperBound);
            this.checkIfEnemyCounterInSecondInterval(secondIntervalLowerBound, secondIntervalUpperBound);
            this.checkIfEnemyCounteBetweenThirdInterval(thirdIntervalLowerBound, thirdIntervalUpperBound);
        }

        private void setEnemyCounterToFirstIntervalAfterLastInterval(int thirdIntervalUpperBound,
            int firstIntervalUpperBound)
        {
            if (this.enemyMotionCounter > thirdIntervalUpperBound)
            {
                this.enemyMotionCounter = firstIntervalUpperBound;
            }
        }

        private void checkIfEnemyCounteBetweenThirdInterval(int secondIntervalLowerBound, int secondIntervalUpperBound)
        {
            if ((this.enemyMotionCounter > secondIntervalLowerBound) &&
                (this.enemyMotionCounter <= secondIntervalUpperBound))
            {
                this.moveAllEnemyShipsRight();
            }
        }

        private void checkIfEnemyCounterInSecondInterval(int secondIntervalLowerBound, int secondIntervalUpperBound)
        {
            if ((this.enemyMotionCounter > 10) && (this.enemyMotionCounter <= 30))
            {
                this.moveAllEnemyShipsLeft();
            }
        }

        private void checkIfEnemyCounterInFirstInterval(int firstIntervalUpperBound)
        {
            if (this.enemyMotionCounter <= firstIntervalUpperBound)
            {
                this.moveAllEnemyShipsRight();
            }
        }

        private void handlePlayerBulletHits()
        {
            var currentfleet = this.fleet.GetAllEnemyShips();
            foreach (var enemy in currentfleet)
            {
                this.killEnemiesHit(enemy);
            }
        }

        private void placePlayerBullet(Bullet bullet)
        {
            double halfOfShip = this.playerShip.Sprite.Width* Half;
            bullet.X = this.playerShip.X + halfOfShip;
            bullet.Y = this.playerShip.Y;
        }

        private void killEnemiesHit(EnemyShip enemy)
        {
            if (this.playerAmmo.Any())
            {
                this.removeEachEnemyHitByBulletFromGame(enemy);
            }
        }

        private void removeEachEnemyHitByBulletFromGame(EnemyShip enemy)
        {
            for (var i = 0; i < this.playerAmmo.Count; i++)
            {
                var bulletFired = this.playerAmmo[i];
                this.removeEnemyHitByBulletFromGame(bulletFired, enemy);
            }
        }

        private void removeEnemyHitByBulletFromGame(Bullet bullet, EnemyShip enemy)
        {
            if (this.bulletHitShip(bullet, enemy))
            {
                this.gameScoreboard.IncreaseScore(enemy.GetLevel);
                this.fleet.RemoveEnemyFromFleet(enemy);
                this.removePlayerBullet(bullet);
                this.currentBackground.Children.Remove(enemy.Sprite);
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
            if (!this.playerPlayerShipLives.IsThereAnyLives || this.areEnemyShipsOutOfPlay())
            {
                return true;
            }
            return false;
        }

        private bool areEnemyShipsOutOfPlay()
        {
            if (
                !(this.fleet.GetAllEnemyShips().Any() &&
                  this.currentBackground.Children.Contains(this.playerShip.Sprite)))
            {
                return true;
            }
            return false;
        }

        private static bool areGameOverConditionsMet(bool enemShipsOutOfPlay, bool playerShipIsOutOfPlay,
            bool gameOver)
        {
            if (enemShipsOutOfPlay || playerShipIsOutOfPlay)
            {
                return true;
            }

            return gameOver;
        }

        private void stopTimerAtGameOver()
        {
            if (this.IsGameOver())
            {
                this.gameTimer.Stop();
            }
        }

        private string displayGameStatistics()
        {
            return "Score: " + this.gameScoreboard.Score + "\n" + "Lives: " + this.playerPlayerShipLives.AmountOfLives;
        }

        #endregion

        #region Property

        /// <summary>
        ///     String GameStatistics records game score and player lives
        /// </summary>
        public string GameStatistics => this.displayGameStatistics();

        /// <summary>
        ///     Records game score
        /// </summary>
        public int GameScore => this.gameScoreboard.Score;

        #endregion
    }
}