using System;
using Android.OS;
using Java.Lang;

namespace PerpetualEngine
{
    public partial class SimpleTimer
    {
        static SimpleTimer()
        {
            SimpleTimer.Create = () => {
                return new DroidSimpleTimer();
            };
        }
    }

    public class DroidSimpleTimer: SimpleTimer
    {
        Handler handler;
        IRunnable restartRunnable;
        bool running = false;

        public DroidSimpleTimer()
        {
            handler = new Handler();
        }

        public override void Repeat(TimeSpan timeSpan, Action action, bool immediate = false)
        {
            Clear();
            running = true;
            if (immediate)
                action();
            Action restartAction = () => {
                action();
                handler.PostDelayed(restartRunnable, (int)timeSpan.TotalMilliseconds);
            };
            restartRunnable = new Runnable(restartAction);
            handler.PostDelayed(restartRunnable, (int)timeSpan.TotalMilliseconds);
        }

        public override void Clear()
        {
            handler.RemoveCallbacks(restartRunnable);
            restartRunnable = null;
            running = false;
        }

        public override bool IsRunning()
        {
            return running;
        }
    }
}