using System;
using Windows.UI.Xaml;

namespace SpaceInvaders.Util
{
    /// <summary>
    /// Defines basics of every game GameTimer
    /// </summary>
    public abstract class TimerUtil
    {
        #region  Data members/Properties
        private const int DefaultTickInterval = 500;
        private readonly TimeSpan gameTickInterval;
        private readonly DispatcherTimer timer;

        /// <summary>
        /// Gets or sets the tick count.
        /// </summary>
        /// <value>
        /// The tick count.
        /// </value>
        public int TickCount { get; set; } = 0;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerUtil"/> class.
        /// </summary>
        protected TimerUtil()
        {
            this.timer = new DispatcherTimer();
            this.gameTickInterval = new TimeSpan(0, 0, 0, 0, DefaultTickInterval);
            this.initializeTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerUtil"/> class.
        /// </summary>
        /// <param name="tickInterval">The tick interval in milliseconds.</param>
        protected TimerUtil(int tickInterval)
        {
            this.timer = new DispatcherTimer();
            this.gameTickInterval = new TimeSpan(0, 0, 0, 0, tickInterval);
            this.initializeTimer();
        }

        private void initializeTimer()
        {
            this.timer.Interval = this.gameTickInterval;
            this.timer.Tick += this.TimerOnTick;
            this.timer.Start();
        }

        /// <summary>
        /// Timers on tick event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public abstract void TimerOnTick(object sender, object e);
    }
}
