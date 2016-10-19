using System;
using Windows.UI.Xaml;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the player ship.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class PlayerShip : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;
        private readonly TimeSpan gameTickInterval = new TimeSpan(0, 0, 0, GameManager.TickInterval, 0);
        private readonly DispatcherTimer timer;

        #endregion

        #region Properties

        #region Property

        public bool Destroyed { get; set; }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip()
        {
            this.timer = new DispatcherTimer {Interval = this.gameTickInterval};
            this.timer.Tick += this.timerOnTick;
            this.timer.Start();

            Sprite = new PlayerShipSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        private void timerOnTick(object sender, object e)
        {
            if (this.Destroyed)
            {
                this.Destroyed = false;
            }
        }

        #endregion
    }
}