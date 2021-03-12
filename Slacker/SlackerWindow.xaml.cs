using Slacker.Sources;
using System;
using System.Windows;
using System.Windows.Input;

namespace Slacker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SlackerWindow : Window
    {
        public SlackerSettings slackerSettings;

        public SlackerWindow()
        {
            InitializeComponent();
            InitializeSystemTray();
            SettingsUIInit();
        }

        private void CloseToTray()
        {
            WindowState = WindowState.Minimized;
            Visibility = Visibility.Hidden;
            ShowInTaskbar = false;
        }

        private void SettingsUIInit()
        {
            try
            {
                slackerSettings = RegistryHandler.LoadSlackerSettings();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            ApplySettings(slackerSettings);

            CloseToTray();

            StartSlacking(slackerSettings);
        }

        private void ApplySettings(SlackerSettings settings)
        {
            if (settings.Defaults)
            {
                slackerSettings.TimeInterval = 59;
                slackerSettings.KeyPressed = Key.F15;
                slackerSettings.FullKeyPress = false;
                DefaultsCheckBox.IsChecked = true;
            }
            
            TimeIntervalBox.Text = slackerSettings.TimeInterval.ToString();
            KeyBox.Text = slackerSettings.KeyPressed.ToString();
            KeypressCheckBox.IsChecked = slackerSettings.FullKeyPress;
        }

        private void ToggleSlacking()
        {
            if (AmISlacking())
            {
                StopSlacking();
                StatusLabel.Content = "Inactive";
                NotifyIcon.Icon = IconHandles["Stopped"];
            }
            else
            {
                StartSlacking(slackerSettings);
                StatusLabel.Content = "Active";
                NotifyIcon.Icon = IconHandles["Working"];
            }
        }

        #region Event Handlers

        private void DefaultsCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (DefaultsCheckBox.IsChecked.HasValue && DefaultsCheckBox.IsChecked.Value)
            {
                slackerSettings.Defaults = true;
                ApplySettings(slackerSettings);
                TimeIntervalBox.IsEnabled = KeyBox.IsEnabled = KeypressCheckBox.IsEnabled = false;
            }
            else
            {
                slackerSettings.Defaults = false;
                TimeIntervalBox.IsEnabled = KeyBox.IsEnabled = KeypressCheckBox.IsEnabled = true;
            }
        }

        private void KeypressCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            slackerSettings.FullKeyPress = KeypressCheckBox.IsChecked ?? false;
        }

        private void KeyBox_KeyDown(object sender, KeyEventArgs e)
        {
            KeyBox.Text = String.Empty;
            KeyBox.Text = e.Key.ToString();
            slackerSettings.KeyPressed = e.Key;

            SettingsSaveButton.Focus();
        }

        private void KeyBox_GotFocus(object sender, RoutedEventArgs e)
        {
            KeyBox.Text = String.Empty;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            CloseToTray();
        }

        private void SettingsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryHandler.SaveSlackerSettings(slackerSettings);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            StopSlacking();
            StartSlacking(slackerSettings);
        }

        private void TimeIntervalBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                slackerSettings.TimeInterval = Int32.Parse(TimeIntervalBox.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StatusToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleSlacking();
        }

        #endregion
    }
}
