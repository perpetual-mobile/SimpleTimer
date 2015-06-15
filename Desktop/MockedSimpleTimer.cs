using System;

namespace PerpetualEngine
{
    public partial class SimpleTimer
    {
        static SimpleTimer()
        {
            SimpleTimer.Create = () => {
                return new MockedSimpleTimer();
            };
        }
    }

    class MockedSimpleTimer : SimpleTimer
    {
        bool running = false;

        public override void Repeat(TimeSpan timeSpan, Action action, bool immediate = false)
        {
            if (immediate)
                action();
            RepeatAsync(timeSpan, action);
        }

        async void RepeatAsync(TimeSpan timeSpan, Action action)
        {
            running = true;
            while (running) {
                await timeSpan;
                if (running) {
                    action();
                }
            }
        }

        public override void Clear()
        {
            running = false;
        }

        public override bool IsRunning()
        {
            return running;
        }
    }
}

