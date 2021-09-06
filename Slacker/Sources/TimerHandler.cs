using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;

namespace Slacker
{
    public partial class SlackerWindow : Window
    {
        private class TimeDuration
        {
            public string Name;
            public TimeSpan Duration;

            public TimeDuration(string name, TimeSpan duration)
            {
                Name = name;
                Duration = duration;
            }
        }

        #region Properties

        private List<TimeDuration> SlackingDurations;
        private Timer ActivityTimer;

        #endregion

        private void InitializeTimeProps()
        {
            SlackingDurations = new List<TimeDuration>();

            SlackingDurations.Add(new TimeDuration("30 Minutes", new TimeSpan(0, 0, 5)));
            SlackingDurations.Add(new TimeDuration("1 Hour", new TimeSpan(0, 0, 10)));
            SlackingDurations.Add(new TimeDuration("2 Hours", new TimeSpan(0, 0, 20)));
            SlackingDurations.Add(new TimeDuration("4 Hours", new TimeSpan(0, 0, 40)));
            SlackingDurations.Add(new TimeDuration("8 Hours", new TimeSpan(0, 0, 60)));

            ActivityTimer = new Timer();
        }

        #region Timer Operations

        private void StartTimer(bool active, TimeSpan duration)
        {
            ActivityTimer.Interval = duration.TotalMilliseconds;
            ActivityTimer.Elapsed += (sender, e) => OnElapsedEvent(sender, e, active);
            ActivityTimer.AutoReset = false;
            ActivityTimer.Start();
        }

        private void StopTimer()
        {
            ActivityTimer.Stop();
        }

        #endregion

        #region Timer Handler

        private void OnElapsedEvent(object sender, ElapsedEventArgs e, bool active)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (active)
                {
                    EndSlacking();
                }
                else
                {
                    BeginSlacking();
                }
            });
        }

        #endregion
    }
}
