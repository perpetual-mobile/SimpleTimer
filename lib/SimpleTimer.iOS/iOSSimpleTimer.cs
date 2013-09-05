using System;
using System.Collections.Generic;
using MonoTouch.Foundation;

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
        struct TimerTemplate
        {
            public TimeSpan TimeSpan;
            public Action Action;
        }

        List<NSTimer> timers;
        List<TimerTemplate> templates;
        ApplicationActivationListener applicationActivationListener;

        public iOSSimpleTimer()
        {
            timers = new List<NSTimer>();
            templates = new List<TimerTemplate>();
            applicationActivationListener = new ApplicationActivationListener();
            applicationActivationListener.OnActivatedAction = () => {
                RescheduleTimers(); };
            applicationActivationListener.OnWillResignActiveAction = () => {
                UnscheduleTimers(); };
        }

        ~ iOSSimpleTimer()
        {
            applicationActivationListener.ClearActions();
        }

        /// <summary>calls the given action with the given time span as long as the App is visible on the screen.</summary>
        public override void Repeat(TimeSpan timeSpan, Action action)
        {
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
            timers.Add(NSTimer.CreateRepeatingScheduledTimer(template.TimeSpan, new NSAction(template.Action)));
        }
    }
}