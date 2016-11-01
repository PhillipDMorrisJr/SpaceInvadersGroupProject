using Windows.UI;
using Windows.UI.Xaml.Media;
using SpaceInvaders.Util;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SpaceInvaders.View.Sprites
{
    /// <summary>
    ///     Draws a level 1 player ship.
    ///     Must implemente the ISpriteRenderer so will be displayed as a game object.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="SpaceInvaders.View.Sprites.ISpriteRenderer" />
    public sealed partial class Level1EnemyShipSprite
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level1EnemyShipSprite" /> class.
        ///     Precondition: none
        ///     Postconditon: Level1EnemyShipSprite created.
        /// </summary>
        public Level1EnemyShipSprite()
        {
            this.InitializeComponent();
            var timer = new AnimationGameTimer(this);
        }

        #endregion

        /// <summary>
        ///     Manages an AnimationGameTimer
        /// </summary>
        /// <seealso cref="SpaceInvaders.Util.TimerUtil" />
        public class AnimationGameTimer : TimerUtil
        {
            private readonly Level1EnemyShipSprite parent;

            /// <summary>
            ///     Initializes a new instance of the <see cref="AnimationGameTimer" /> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            public AnimationGameTimer(Level1EnemyShipSprite parent)
            {
                this.parent = parent;
            }

            /// <summary>
            ///     Timers on tick event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The e.</param>
            public override void TimerOnTick(object sender, object e)
            {
                if (TickCount%2 == 0)
                {
                    parent.domeTop.Fill = new SolidColorBrush(Colors.Gainsboro);
                    parent.domeBottom.Fill = new SolidColorBrush(Colors.Gainsboro);
                }
                else
                {
                    parent.domeTop.Fill = new SolidColorBrush(Colors.DimGray);
                    parent.domeBottom.Fill = new SolidColorBrush(Colors.DimGray);
                }
                TickCount++;
            }
        }

        #region Methods

        #endregion
    }
}