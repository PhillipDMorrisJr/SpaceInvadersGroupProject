﻿using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the EnemyShip class.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class EnemyShip : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the get level.
        /// </summary>
        /// <value>
        ///     The get level.
        /// </value>
        public int Level { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has fired.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has fired; otherwise, <c>false</c>.
        /// </value>
        public bool HasFired { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class base level 1.
        /// </summary>
        public EnemyShip() : this(1)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class.
        ///     Precondition: level must be at least 1.
        ///     Postcondition: new enemyShip created
        /// </summary>
        /// <param name="level">The level.</param>
        public EnemyShip(int level)
        {
            if (level < 0)
            {
                level = 1;
            }
            Level = level;

            switch (level)
            {
                case 0:
                    Sprite = new BonusEnemyShipSprite();
                    SetSpeed(SpeedXDirection, SpeedYDirection);
                    break;
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

        #region Methods

        #endregion
    }
}