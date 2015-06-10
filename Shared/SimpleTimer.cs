using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PerpetualEngine
{
    public abstract partial class SimpleTimer
    {
        /// <summary>
        /// </summary>
        /// <value></value>
        public static Func<SimpleTimer> Create { get; set; }

        /// <summary>Schedule timer.</summary>
        /// <param name="timeSpan">Trigger interval.</param>
        /// <param name="action">To be executed when timer triggers.</param>
        public abstract void Repeat(TimeSpan timeSpan, Action action, bool immediate = false);

        /// <summary>
        /// Remove timers.
        /// Callback actions will not be triggered any more after this call (no pending triggers).
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Return if the timer is currently Running.
        /// </summary>
        public abstract bool IsRunning();

        /// <summary>
        /// Tells timer not to handle background/foreground transition of app.
        /// Currently only supported on iOS.
        /// </summary>
        public virtual void DisableAutomaticBackgroundHandling()
        {
        }

    }
}