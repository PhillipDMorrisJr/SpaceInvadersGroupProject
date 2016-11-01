using System;
using Windows.UI.Xaml;

namespace SpaceInvaders.Util
{
    /// <summary>
    ///     Defines basics of every game GameTimer
    /// </summary>
    public abstract class TimerUtil
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TimerUtil" /> class.
        /// </summary>
        protected TimerUtil()
        {
            timer = new DispatcherTimer();
            gameTickInterval = new TimeSpan(0, 0, 0, 0, DefaultTickInterval);
            initializeTimer();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimerUtil" /> class.
        /// </summary>
        /// <param name="tickInterval">The tick interval in milliseconds.</param>
        protected TimerUtil(int tickInterval)
        {
            timer = new DispatcherTimer();
            gameTickInterval = new TimeSpan(0, 0, 0, 0, tickInterval);
            initializeTimer();
        }

        private void initializeTimer()
        {
            timer.Interval = gameTickInterval;
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        /// <summary>
        ///     Timers on tick event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public abstract void TimerOnTick(object sender, object e);

        #region  Data members/Properties

        private const int DefaultTickInterval = 500;
        private readonly TimeSpan gameTickInterval;
        private readonly DispatcherTimer timer;

        /// <summary>
        ///     Gets or sets the tick count.
        /// </summary>
        /// <value>
        ///     The tick count.
        /// </value>
        public int TickCount { get; set; } = 0;

        #endregion
    }
}