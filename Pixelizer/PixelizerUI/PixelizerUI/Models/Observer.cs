using Avalonia;
using Avalonia.Controls;
using System;

namespace PixelizerUI.Models
{
    public class ClientSizeObserver : IObserver<AvaloniaPropertyChangedEventArgs<Size>>
    {
        private readonly Window _window;
        private readonly Action<Size> _observer;

        public ClientSizeObserver(Window window, Action<Size> observer)
        {
            _window = window;
            _observer = observer;
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(AvaloniaPropertyChangedEventArgs<Size> value)
        {
            if (value.Sender == _window)
            {
                _observer(value.NewValue.Value);
            }
        }
    }
}
