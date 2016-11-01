namespace SpaceInvaders.View.Sprites
{
    /// <summary>
    ///     Draws a player ship.
    ///     Must implemente the ISpriteRenderer so will be displayed as a game object.
    /// </summary>
    public sealed partial class PlayerShipSprite
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShipSprite" /> class.
        ///     Precondition: none
        ///     Postconditon: Sprite created.
        /// </summary>
        public PlayerShipSprite()
        {
            this.InitializeComponent();
        }

        #endregion

        
    }
}