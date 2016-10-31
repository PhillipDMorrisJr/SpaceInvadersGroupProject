using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.Sprites
{
    /// <summary>
    ///     Draws a level 2 ship.
    ///     Must implemente the ISpriteRenderer so will be displayed as a game object.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="SpaceInvaders.View.Sprites.ISpriteRenderer" />
    public sealed partial class Level2EnemyShipSprite : AbstractUserControl
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level2EnemyShipSprite" /> class.
        ///     Precondition: none
        ///     Postconditon: Level2EnemyShipSprite sprite created.
        /// </summary>
        public Level2EnemyShipSprite()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        

        #endregion
    }
}