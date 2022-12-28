using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace PixelizerUI.Views
{
    public partial class MainWindow : Window
    {
        public WindowNotificationManager Manager { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Manager = new WindowNotificationManager(this)
            {
                Position = NotificationPosition.BottomLeft,
                MaxItems = 3,
            };
        }
    }
}