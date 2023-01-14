using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using PixelizerUI.ViewModels;
using System;

namespace PixelizerUI.Views
{
    public partial class DialogWindow : Window
    {
        public INotificationManager NotificationManager { get; }

        public DialogWindow()
        {
            InitializeComponent();

            Closing += DialogWindow_Closing;

            NotificationManager = new WindowNotificationManager(this)
            {
                Position = NotificationPosition.TopLeft,
                MaxItems = 1,
            };
        }

        private void DialogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is IClose close)
            {
                e.Cancel = !close.CanClose;
            }
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);

            if (DataContext is IClose close)
            {
                close.OnClose += Close;
            }
        }
    }
}
