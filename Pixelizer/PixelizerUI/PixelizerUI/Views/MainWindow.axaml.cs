using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace PixelizerUI.Views
{
    public partial class MainWindow : Window
    {
        public WindowNotificationManager Manager { get; private set; }

        private bool _wasPressed;
        private Point? _lastPosition;
        private double _comparatorPos;

        public MainWindow()
        {
            InitializeComponent();

            Manager = new WindowNotificationManager(this)
            {
                Position = NotificationPosition.TopRight,
                MaxItems = 3,
            };

            ComparatorControl.PointerPressed += Rectangle_PointerPressed;
            ComparatorControl.PointerReleased += Rectangle_PointerReleased;
            ComparatorControl.PointerMoved += Rectangle_PointerMoved;
        }

        private void Rectangle_PointerReleased(object sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            _wasPressed = false;
            _lastPosition = null;
            e.Handled = true;
        }

        private void Rectangle_PointerMoved(object sender, Avalonia.Input.PointerEventArgs e)
        {
            if (_wasPressed is false || WidthSlider.IsEnabled is false)
            {
                return;
            }

            Point position = e.GetPosition(this);
            _comparatorPos += position.X - _lastPosition.Value.X;
            WidthSlider.Value = _comparatorPos;
            _lastPosition = position;

            e.Handled = true;
        }

        private void Rectangle_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            _wasPressed = true;
            _lastPosition = e.GetPosition(this);
            _comparatorPos = WidthSlider.Value;
            e.Handled = true;
        }
    }
}