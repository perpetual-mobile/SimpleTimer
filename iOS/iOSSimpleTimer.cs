using System;
using System.Collections.Generic;
using Foundation;

namespace PerpetualEngine
{
    /// <summary>
    /// iOS specific implementation to let EditGroup(string) return a n iOSSimpleStorage object
    /// </summary>
    public partial class SimpleTimer
    {
        static SimpleTimer()
        {
            SimpleTimer.Create = () => {
                return new iOSSimpleTimer();
            };
        }
    }

    public class iOSSimpleTimer : SimpleTimer
    {
        public static bool UseApplicationActivationListener = true;

        struct TimerTemplate
        {
            public TimeSpan TimeSpan;
            public Action Action;
        }

        List<NSTimer> timers;
        List<TimerTemplate> templates;
        ApplicationActivationListener applicationActivationListener;
        bool running = false;

        public iOSSimpleTimer()
        {
            timers = new List<NSTimer>();
            templates = new List<TimerTemplate>();
            if (UseApplicationActivationListener) {
                applicationActivationListener = new ApplicationActivationListener();
                applicationActivationListener.OnActivatedAction = () => {
                    TriggerTimerActions();
                    RescheduleTimers();
                };
                applicationActivationListener.OnWillResignActiveAction = () => {
                    UnscheduleTimers();
                };
            }
        }

        ~ iOSSimpleTimer()
        {
            if (UseApplicationActivationListener)
                applicationActivationListener.ClearActions();
        }

        /// <summary>calls the given action with the given time span as long as the App is visible on the screen.</summary>
        public override void Repeat(TimeSpan timeSpan, Action action, bool immediate = false)
        {
            running = true;
            if (immediate)
                action();
            TimerTemplate template;
            template.TimeSpan = timeSpan;
            template.Action = action;
            templates.Add(template);
            ScheduleTimer(template);
        }

        public override void Clear()
        {
            UnscheduleTimers();
            ClearTemplates();
            running = false;
        }

        public override bool IsRunning()
        {
            return running;
        }

        public override void DisableAutomaticBackgroundHandling()
        {
            applicationActivationListener.ClearActions();
        }

        void UnscheduleTimers()
        {
            foreach (var timer in timers) {
                timer.Invalidate();
            }
            timers.Clear();
        }

        void RescheduleTimers()
        {
            UnscheduleTimers();
            ScheduleTimers();
        }

        void ScheduleTimers()
        {
            foreach (var template in templates) {
                ScheduleTimer(template);
            }
        }

        void ClearTemplates()
        {
            templates.Clear();
        }

        void ScheduleTimer(TimerTemplate template)
        {
            timers.Add(NSTimer.CreateRepeatingScheduledTimer(template.TimeSpan, new Action<NSTimer>((NSTimer arg) => template.Action())));
        }

        void TriggerTimerActions()
        {
            // work on copy of templates list
            // we want to allow modification of templates in template action
            foreach (var template in templates.ToArray()) {
                template.Action();
            }
        }
    }
}