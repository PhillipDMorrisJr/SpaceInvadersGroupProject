using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Util;

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
        private EnemyShip bonusEnemyShip;

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

            this.currentLevel = 1;
            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;
            

            this.initializePlayerShipFactory();

            this.fleet = new EnemyFleet(4);

            this.initializeDataMembers();

            this.enemyMotionCounter = 0;

            this.initializeTimer();  
        }

        private void initializeDataMembers()
        {
            this.playerAmmo = new List<Bullet>();
            this.enemyAmmo = new List<Bullet>();

            this.gameScoreboard = new Scoreboard();
        }

        private void initializePlayerShipFactory()
        {
            this.playerPlayerShipFactory = new PlayerShipFactory(3);
            this.playerShip = this.playerPlayerShipFactory.UseLife();
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
            this.handleBonusShip();

            // this.bonusEnemyShip.MoveLeft();
            this.MoveEnemyShips();
            this.FireEnemyBullet();
            this.HandleBullets();
            this.stopTimerAtGameOver();
             
        }

        private void handleBonusShip()
        {

            this.addBonusEnemyToGame();
            this.moveBonusEnemyShip();


        }

        /// <summary>
        ///     Manages player bullet when fired
        ///     Precondition: There must be bullets in player ammo
        ///     Postcondition: Bullet is fired
        /// </summary>
        public void FirePlayerBullet()
        {
            var isPlayerWaiting = this.checkIfPlayerIsWaitingToFire();
            this.checkIfPlayerAmmoShouldBeStocked(isPlayerWaiting);
            this.handlePlayerBulletHits();
        }

        private bool checkIfPlayerIsWaitingToFire()
        {
            bool isBulletWaiting = false;
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
            List<Bullet> enemyBulletsToRemove = new List<Bullet>(this.enemyAmmo);
            foreach (Bullet enemyBullet in enemyBulletsToRemove)
            {
                this.moveEnemyBulletDownWhenBulletIsInCanvas(enemyBullet);
                this.removeEnemyBulletFromPlayWhenItHitsPlayerShip(enemyBullet);
            }
        }


        private void removeEnemyBulletFromPlayWhenItHitsPlayerShip(Bullet enemyBullet)
        {
            if (this.bulletHitShip(enemyBullet, this.playerShip))
            {
                this.currentBackground.Children.Remove(this.playerShip.Sprite);
                this.removeBulletFromGame(enemyBullet);

                this.bringPlayerBackToLifeWhenThereAreMoreLives();
            }
        }

        private void bringPlayerBackToLifeWhenThereAreMoreLives()
        {
            this.gameTimer.Stop();
            this.playerShip.Destroyed = true;

            if (this.playerPlayerShipFactory.IsThereAnyLives)
            {
                if (this.playerShip.Destroyed)
                {
                    this.playerShip = this.playerPlayerShipFactory.UseLife();
                    this.addPlayerShipToGame();
                    this.gameTimer.Start();
                }
            }
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
            var firingShips = this.fleet.GetFiringShips();

            this.fireFiringShips(firingShips);
        }

        private void fireFiringShips(List<EnemyShip> firingShips)
        {
            if (firingShips.Any())
            {
                this.fireMultipleShips(firingShips);
            }
        }

        private void fireMultipleShips(List<EnemyShip> firingShips)
        {
            var amountOfShipsToFire = RandomUtil.GetNextRandomFromMax(firingShips.Count());
            for (var i = 0; i < amountOfShipsToFire; i++)
            {
                var firingShip = this.selectFiringShip(amountOfShipsToFire, firingShips);
                this.fireShipWhenShipHasNotFired(firingShip);
            }
        }

        private void fireShipWhenShipHasNotFired(EnemyShip firingShip)
        {
            

            var isEnemyWaiting = this.checkIfEnemyIsWaiting(firingShip);
            if (!this.enemyAmmo.Any() || !isEnemyWaiting)
            {
                this.addEnemyBulletsToScreen(firingShip);
                firingShip.HasFired = true;
            }
        }

        private bool checkIfEnemyIsWaiting(EnemyShip enemy)
        {
            bool isEnemyWaiting = false;

            if (this.enemyAmmo.Any())
            {
                var bulletClosetToShip = this.enemyAmmo.Max(bullet => bullet.Y + bullet.Height);

                isEnemyWaiting = bulletClosetToShip < enemy.Y - enemy.Height;
            }
            return isEnemyWaiting;
        }

        private EnemyShip selectFiringShip(int amountOfShipsToFire,
            List<EnemyShip> firingShips)
        {
            var firingShipIndex = RandomUtil.GetNextRandomFromMax(amountOfShipsToFire);
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
            this.handlePlayerBullets();
            this.handlePlayerBulletHits();
        }

        private void handlePlayerBullets()
        {
            List<Bullet> playerBulletsToRemove = new List<Bullet>(this.playerAmmo);
                foreach (var bullet in playerBulletsToRemove)
            {
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
                this.removeBulletFromGame(bullet);
            }
        }

        private void removeBulletFromGame(Bullet bullet)
        {
            this.currentBackground.Children.Remove(bullet.Sprite);
            if (this.playerAmmo.Contains(bullet))
            {
                this.playerAmmo.Remove(bullet);
            } else if (this.enemyAmmo.Contains(bullet))
            {
                this.enemyAmmo.Remove(bullet);
            }
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
            for (var levelIterator = 1; levelIterator <= this.fleet.Levels; levelIterator++)
            {
                
                var enemyRowCount = this.fleet.GetAmountOfShipForLevel(levelIterator);
                var enemyXOrigin = this.calculateEnemyXOrigin(enemyRowCount);

                this.setXLocationForShipsAfterFirstShip(levelIterator, enemyXOrigin);
            }
        }

        private double calculateEnemyXOrigin( int enemyRowCount)
        {
            var levelOneShip = this.fleet.GetAllEnemyShips().First();
            return this.backgroundWidth*Half - levelOneShip.Width*(enemyRowCount*Half) -
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
            if (level == 1)
            {
                currentEnemy.Y = this.backgroundHeight*Half;
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
            this.playerShip.X = (this.backgroundWidth - this.playerShip.Width)*Half;
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

            var secondIntervalUpperBound = firstIntervalUpperBound + interval;
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
            if ((this.enemyMotionCounter > secondIntervalLowerBound) && (this.enemyMotionCounter <= secondIntervalUpperBound))
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

        private void addBonusEnemyToGame()
        {
            var randomInt = RandomUtil.GetNextRandomFromMax(30);
            if (this.bonusEnemyShip == null)
            {
                this.bonusEnemyShip = (EnemyShip)ShipFactory.SelectShip(ShipFactory.ShipSelections.BonusShip);
                this.currentBackground.Children.Add(this.bonusEnemyShip.Sprite);
                this.placeBonusEnemyShip();
            }
        }

        private void placeBonusEnemyShip()
        {
            this.bonusEnemyShip.X = this.currentBackground.Width - this.bonusEnemyShip.Width;
            this.bonusEnemyShip.Y = this.bonusEnemyShip.Height;
        }

        private void moveBonusEnemyShip()
        {
            if (this.currentBackground.Children.Contains(this.bonusEnemyShip.Sprite))
            {
                var backgroundOrigin = 0;

                if (this.bonusEnemyShip.X > backgroundOrigin)
                {
                    this.bonusEnemyShip.MoveLeft();
                }
                else
                {
                    this.removeBonusShipFromGame();
                } 
            }
        }

        private void removeBonusShipFromGame()
        {
            this.currentBackground.Children.Remove(this.bonusEnemyShip.Sprite);
            this.bonusEnemyShip = null;
            this.BonusEnemyShipVisible = false;
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
            var halfOfShip = this.playerShip.Sprite.Width*Half;
            bullet.X = this.playerShip.X + halfOfShip - bullet.Sprite.Width * Half;
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
            List<Bullet> playerBulletsFired = new List<Bullet>(this.playerAmmo);
            foreach (var bulletFired in playerBulletsFired)
            {
                this.removeEnemyHitByBulletFromGame(bulletFired, enemy);

                this.checkBonusShipHit(bulletFired);
            }
        }

        private void checkBonusShipHit(Bullet bulletFired)
        {
            if (this.bonusEnemyShip != null && this.currentBackground.Children.Contains(this.bonusEnemyShip.Sprite)
            
        )
            {
                this.removeEnemyHitByBulletFromGame(bulletFired, this.bonusEnemyShip);
            }
        }

        private void removeEnemyHitByBulletFromGame(Bullet bullet, EnemyShip enemy)
        {
            if (this.bulletHitShip(bullet, enemy))
            {
                if (enemy.Level == 0)
                {
                    this.gameScoreboard.IncreaseScore(10000);
                    this.currentBackground.Children.Remove(this.bonusEnemyShip.Sprite);
                }
                this.gameScoreboard.IncreaseScore(enemy.Level);
                this.fleet.RemoveEnemyFromFleet(enemy);
                this.removeBulletFromGame(bullet);
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

            return bulletInXBound && bulletInYBound;
        }

        /// <summary>
        ///     Returns true if the game is over
        ///     Precondition: none
        ///     PostCondition: true is returned if the game has ended
        /// </summary>
        /// <returns>return true when the game is over</returns>
        public bool IsGameOver()
        {
            
            if (!this.playerPlayerShipFactory.IsThereAnyLives || this.areEnemyShipsOutOfPlay())
            {
                if (this.currentLevel < 3 && this.playerPlayerShipFactory.IsThereAnyLives)
                {
                    this.currentLevel++;
                    this.gameTimer.Stop();
                    this.goToNextRound();
                    return false;
                }
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

        private void stopTimerAtGameOver()
        {
            if (this.IsGameOver())
            {
                this.resetCanvas();
                this.gameTimer.Stop();
            }
        }

        private void goToNextRound()
        {
            this.resetCanvas();

            this.initializePlayerShipFactory();

            this.fleet = new EnemyFleet(this.fleet.Levels + 1);

            this.initializeDataMembers();

            this.enemyMotionCounter = 0;

            this.initializeTimer();
            this.InitializeGame(this.currentBackground);
        }

        private string displayGameStatistics()
        {
            return "Score: " + this.gameScoreboard.Score + "\n" + "Lives: " + this.playerPlayerShipFactory.AmountOfLives;
        }

        private void resetCanvas()
        {
            this.currentBackground.Children.Remove(this.playerShip.Sprite);
            this.clearEnemies();

            this.clearBullets();
        }

        private void clearBullets()
        {
            this.clearEnemyBullets();
            this.clearPlayerBullets();
        }

        private void clearPlayerBullets()
        {

            List<Bullet> playerBulletsToRemove = new List<Bullet>(this.playerAmmo);
            foreach (var bullet in playerBulletsToRemove)
            {
                this.removeBulletFromGame(bullet);
            }
        }

        private void clearEnemyBullets()
        {
            List<Bullet> enemyBulletsToRemove = new List<Bullet>(this.enemyAmmo);
            foreach (var bullet in enemyBulletsToRemove)
            {
                this.removeBulletFromGame(bullet);
            }
        }

        private void clearEnemies()
        {
            List<EnemyShip> enemiesToRemove = new List<EnemyShip>(this.fleet.GetAllEnemyShips());

            foreach (EnemyShip enemy in enemiesToRemove)
            {
                this.fleet.RemoveEnemyFromFleet(enemy);
                this.currentBackground.Children.Remove(enemy.Sprite);
            }
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

        /// <summary>
        /// Gets a value indicating whether [bonus enemy ship visible].
        /// </summary>
        /// <value>
        /// <c>true</c> if [bonus enemy ship visible]; otherwise, <c>false</c>.
        /// </value>
        public bool BonusEnemyShipVisible { get; set; } = false;

        #endregion
    }
}