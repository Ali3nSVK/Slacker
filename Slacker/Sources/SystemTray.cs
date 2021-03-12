using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace Slacker
{
    public partial class SlackerWindow : Window
    {
        private NotifyIcon NotifyIcon = null;
        private Dictionary<string, System.Drawing.Icon> IconHandles = null;

        private void InitializeSystemTray()
        {
            InitializeIcons();

            NotifyIcon = new NotifyIcon();
            NotifyIcon.ContextMenuStrip = InitializeContextMenu();
            NotifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            NotifyIcon.Icon = IconHandles["Working"];
            NotifyIcon.Visible = true;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ToggleSlacking();
        }

        private void ContextMenuSettings_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
            Visibility = Visibility.Visible;
            ShowInTaskbar = true;
            SystemCommands.RestoreWindow(this);
        }

        private void ContextMenuExit_Click(object sender, EventArgs e)
        {
            StopSlacking();
            NotifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        private ContextMenuStrip InitializeContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            contextMenu.Items.Add("Settings", null, this.ContextMenuSettings_Click);
            contextMenu.Items.Add("Exit", null, this.ContextMenuExit_Click);

            return contextMenu;
        }

        private void InitializeIcons()
        {
            IconHandles = new Dictionary<string, System.Drawing.Icon>
            {
                { "Working", new System.Drawing.Icon(System.Windows.Application.GetResourceStream(new Uri("/Resources/working-icon.ico", UriKind.Relative)).Stream) },
                { "Stopped", new System.Drawing.Icon(System.Windows.Application.GetResourceStream(new Uri("/Resources/stopped-icon.ico", UriKind.Relative)).Stream) }
            };
        }
    }
}
