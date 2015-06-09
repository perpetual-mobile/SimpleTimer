using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace PerpetualEngine
{
    /// <summary>
    /// Listens to iOS events when app comes to foreground or disappears. 
    /// This is for example used to register/deregister timers.
    /// </summary>
    /// <see cref="CASLib.iOS.TimerController"/>
    public class ApplicationActivationListener
    {
        public delegate void Action();

        public Action OnActivatedAction { get; set; }

        public Action OnWillResignActiveAction { get; set; }

        List<NSObject> notificationTokens;

        public ApplicationActivationListener()
        {
            notificationTokens = new List<NSObject>();
            SubscribeToApplicationNotifications();
        }

        ~ ApplicationActivationListener()
        {
            UnsubscribeFromApplicationNotifications();
        }

        public void ClearActions()
        {
            OnActivatedAction = null;
            OnWillResignActiveAction = null;
        }

        void SubscribeToApplicationNotifications()
        {
            notificationTokens.Add(
                SubscribeToApplicationNotification(UIApplication.DidBecomeActiveNotification, OnApplicationActivated));
            notificationTokens.Add(
                SubscribeToApplicationNotification(UIApplication.WillResignActiveNotification, OnApplicationWillResignActive));
        }

        void UnsubscribeFromApplicationNotifications()
        {
            foreach (var token in notificationTokens) {
                token.Dispose();
            }
            notificationTokens.Clear();
        }

        NSObject SubscribeToApplicationNotification(NSString notificationName, Action<NSNotification> actionToPerform)
        {
            return NSNotificationCenter.DefaultCenter.AddObserver(
                notificationName, actionToPerform, UIApplication.SharedApplication);
        }

        void OnApplicationActivated(NSNotification notification)
        {
            if (OnActivatedAction != null) {
                OnActivatedAction();
            }
        }

        void OnApplicationWillResignActive(NSNotification notification)
        {
            if (OnWillResignActiveAction != null) {
                OnWillResignActiveAction();
            }
        }
    }
}

