using System;
using System.Threading;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    /// Manages the EnemyShip class.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class EnemyShip : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;
        private int enemyLevel;
        /// <summary>
        ///     The tick interval in seconds
        /// </summary>
        public const int TickInterval = 25;
        private readonly TimeSpan gameTickInterval = new TimeSpan(0, 0, 0, 0, TickInterval);
        private readonly DispatcherTimer timer;
        #endregion

        #region Constructors

        public EnemyShip() : this(1)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyShip" /> class.
        /// </summary>
        /// <param name="level">The level.</param>
        public EnemyShip(int level)
        {
            
            if (level < 1)
            {
                level = 1;
            }

            this.timer = new DispatcherTimer { Interval = this.gameTickInterval };

            this.timer.Tick += this.timerOnTick;
            this.timer.Start();

            this.enemyLevel = level;
            switch (level)
            {
                    
                case 1:
                    Sprite = new Level1EnemyShipSprite();
                    SetSpeed(SpeedXDirection, SpeedYDirection);
                    break;
                case 2:
                    Sprite = new Level2EnemyShipSprite();
                    SetSpeed(SpeedXDirection, SpeedYDirection);
                    break;
                case 3:
                    Sprite = new Level3EnemyShipSprite();
                    SetSpeed(SpeedXDirection, SpeedYDirection);
                    break;
                default:
                    Sprite = new Level3EnemyShipSprite();
                    SetSpeed(SpeedXDirection, SpeedYDirection);
                    break;
            }
        }



        #endregion

        private void timerOnTick(object sender, object e)
        {


            if (this.HasFired)
            {
                    this.HasFired = false;
                
            }

            if (this.Sprite.Visibility == Visibility.Collapsed)
            {

                this.Sprite.Visibility = Visibility.Visible;
                
            } else 
            {
                Sprite.Visibility = Visibility.Collapsed;
               
            }
        }




        #region Properties

        public int GetLevel => this.enemyLevel;

        public bool HasFired { get; set; }

        #endregion


    }
}