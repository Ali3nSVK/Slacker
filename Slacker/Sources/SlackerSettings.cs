using System.Windows.Input;

namespace Slacker.Sources
{
    public struct SlackerSettings
    {
        public bool Defaults;
        public int? TimeInterval;
        public Key KeyPressed;
        public bool FullKeyPress;

        public SlackerSettings(bool defaults, int timeInterval, Key keyPressed, bool fullKeyPress)
        {
            Defaults = defaults;
            TimeInterval = timeInterval;
            KeyPressed = keyPressed;
            FullKeyPress = fullKeyPress;
        }
    }
}
