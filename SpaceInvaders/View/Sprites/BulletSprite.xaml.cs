using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.Sprites
{
    /// <summary>
    ///     Draws a player bullet.
    ///     Must implemente the ISpriteRenderer so will be displayed as a game object.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="SpaceInvaders.View.Sprites.ISpriteRenderer" />
    public sealed partial class BulletSprite : UserControl, ISpriteRenderer
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the<see cref="BulletSprite" /> class.
        ///     Precondition: none
        ///     Postconditon: PlayerBulletSprite sprite created.
        /// </summary>
        public BulletSprite()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerBulletSprite" /> class.
        ///     Precondition: none
        ///     Postconditon: PlayerBulletSprite Sprite created.
        /// </summary>
        /// <summary>
        ///     Renders the sprite at the specified location.
        ///     Precondition: none
        ///     Postcondition: sprite drawn at location (x,y)
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void RenderAt(double x, double y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

        #endregion
    }
}