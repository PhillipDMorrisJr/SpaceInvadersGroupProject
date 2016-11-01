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
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip()
        {
            var timer = new DispatcherTimer {Interval = gameTickInterval};
            timer.Tick += timerOnTick;
            timer.Start();

            Sprite = new PlayerShipSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Property

        /// <summary>
        ///     Returns boolean if playerShip is destroyed
        /// </summary>
        public bool Destroyed { get; set; }

        #endregion

        #region Methods

        private void timerOnTick(object sender, object e)
        {
            if (Destroyed)
            {
                Destroyed = false;
            }
        }

        #endregion

        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;
        private readonly TimeSpan gameTickInterval = new TimeSpan(0, 0, 0, GameManager.TickInterval, 0);

        #endregion
    }
}