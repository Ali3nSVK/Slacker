using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace Slacker
{
    public partial class SlackerWindow : Window
    {
        #region Properties

        private NotifyIcon NotifyIcon = null;
        private Dictionary<string, System.Drawing.Icon> IconHandles = null;

        #endregion

        #region Initializers

        private void InitializeSystemTray()
        {
            InitializeIcons();

            NotifyIcon = new NotifyIcon();
            NotifyIcon.ContextMenuStrip = InitializeContextMenu();
            NotifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            NotifyIcon.Icon = IconHandles["Working"];
            NotifyIcon.Visible = true;
        }

        private ContextMenuStrip InitializeContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            contextMenu.Items.Add(InitializeActiveSubmenu());
            contextMenu.Items.Add(InitializeInactiveSubmenu());
            contextMenu.Items.Add("Settings", null, this.ContextMenuSettings_Click);
            contextMenu.Items.Add("Exit", null, this.ContextMenuExit_Click);

            return contextMenu;
        }

        private ToolStripMenuItem InitializeActiveSubmenu()
        {
            ToolStripMenuItem activeSubMenu = new ToolStripMenuItem("Active for");

            foreach (TimeDuration duration in SlackingDurations)
            {
                ToolStripItem item = activeSubMenu.DropDownItems.Add(duration.Name);
                item.Click += (sender, e) => ToolStripMenuActive_Click(sender, e, duration.Duration);
            }

            return activeSubMenu;
        }

        private ToolStripMenuItem InitializeInactiveSubmenu()
        {
            ToolStripMenuItem inactiveSubMenu = new ToolStripMenuItem("Inactive for");

            foreach (TimeDuration duration in SlackingDurations)
            {
                ToolStripItem item = inactiveSubMenu.DropDownItems.Add(duration.Name);
                item.Click += (sender, e) => ToolStripMenuInactive_Click(sender, e, duration.Duration);
            }

            return inactiveSubMenu;
        }

        private void InitializeIcons()
        {
            IconHandles = new Dictionary<string, System.Drawing.Icon>
            {
                { "Working", new System.Drawing.Icon(System.Windows.Application.GetResourceStream(new Uri("/Resources/working-icon.ico", UriKind.Relative)).Stream) },
                { "Stopped", new System.Drawing.Icon(System.Windows.Application.GetResourceStream(new Uri("/Resources/stopped-icon.ico", UriKind.Relative)).Stream) }
            };
        }

        #endregion

        #region Event Handlers

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
            TerminateSlacking();
            NotifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        private void ToolStripMenuActive_Click(object sender, EventArgs e, TimeSpan duration)
        {
            if (!AmISlacking())
            {
                BeginSlacking();
            }

            StartTimer(true, duration);
        }

        private void ToolStripMenuInactive_Click(object sender, EventArgs e, TimeSpan duration)
        {
            if (AmISlacking())
            {
                EndSlacking();
            }

            StartTimer(false, duration);
        }

        #endregion
    }
}
