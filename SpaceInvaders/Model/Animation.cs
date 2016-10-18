using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    class Animation : GameObject
    {
        /// <summary>
        ///     The tick interval in seconds
        /// </summary>
        public const int TickInterval = 25;
        private readonly TimeSpan gameTickInterval = new TimeSpan(0, 0, 0, 0, TickInterval);
        private readonly DispatcherTimer timer;
        public Animation(UserControl UI, int newXLocation, int newYLocation)
        {
            Sprite = UI;
            this.X = newXLocation;
            this.Y = newYLocation;

        }
    }
}
