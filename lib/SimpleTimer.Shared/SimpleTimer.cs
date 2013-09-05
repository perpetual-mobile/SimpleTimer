using System;
using System.Xml.Serialization;
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
        public abstract void Repeat(TimeSpan timeSpan, Action action);

        /// <summary>
        /// Remove timers.
        /// Callback actions will not be triggered any more after this call (no pending triggers).
        /// </summary>
        public abstract void Clear();
    }
}