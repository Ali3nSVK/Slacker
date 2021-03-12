using Slacker.Sources;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Slacker
{
    public partial class SlackerWindow : Window
    {
        private static readonly int MsInSec = 1000;
        private static readonly bool KeepRunning = true;
        private Thread SlackerThread;

        public void StartSlacking(SlackerSettings settings)
        {
            SlackerThread = StartSlackerThread(settings.TimeInterval.Value, settings.KeyPressed, settings.FullKeyPress);
        }

        public void StopSlacking()
        {
            if (AmISlacking())
            {
                SlackerThread.Abort();
            }
        }

        public bool AmISlacking()
        {
            if (SlackerThread == null)
            {
                return false;
            }

            return SlackerThread.IsAlive;
        }

        private Thread StartSlackerThread(int timeInterval, Key keyPressed, bool fullKeyPress)
        {
            var slackerThread = new Thread(() => StartThreadedSlacking(timeInterval, keyPressed, fullKeyPress));
            slackerThread.Start();

            return slackerThread;
        }

        private static void StartThreadedSlacking(int timeInterval, Key keyPressed, bool fullKeyPress)
        {
            KeyboardInvoker invoke = new KeyboardInvoker(fullKeyPress, keyPressed);

            while (KeepRunning)
            {
                invoke.SendInputWithAPI();
                Thread.Sleep(timeInterval * MsInSec);
            }
        }
    }
}
