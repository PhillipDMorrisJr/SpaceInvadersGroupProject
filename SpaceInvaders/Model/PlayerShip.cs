﻿using System;
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
        /// <summary>
        ///     The tick interval in seconds
        /// </summary>
        public const int TickInterval = 5;
        private readonly TimeSpan gameTickInterval = new TimeSpan(0, 0, 0, TickInterval, 0);
        private readonly DispatcherTimer timer;
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip()
        {

            this.timer = new DispatcherTimer { Interval = this.gameTickInterval };

            this.timer.Tick += this.timerOnTick;
            this.timer.Start();

            Sprite = new PlayerShipSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        private void timerOnTick(object sender, object e)
        {

            if (this.Destroyed)
            {
                this.Destroyed = false;

            }
        }

        #region Property

        public bool Destroyed { get; set; } = false;

        #endregion
    }
}