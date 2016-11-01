using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.View.Sprites
{
    /// <summary>
    /// </summary>
    public abstract partial class AbstractUserControl : UserControl, ISpriteRenderer
    {
        #region Methods

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