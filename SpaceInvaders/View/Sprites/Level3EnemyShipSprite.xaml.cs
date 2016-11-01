using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.Sprites
{
    /// <summary>
    ///     Draws a Level 3 ship.
    ///     Must implemente the ISpriteRenderer so will be displayed as a game object.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="SpaceInvaders.View.Sprites.ISpriteRenderer" />
    public sealed partial class Level3EnemyShipSprite
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level3EnemyShipSprite" /> class.
        ///     Precondition: none
        ///     Postconditon: Level3EnemyShipSprite sprite created.
        /// </summary>
        public Level3EnemyShipSprite()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        

        #endregion
    }
}