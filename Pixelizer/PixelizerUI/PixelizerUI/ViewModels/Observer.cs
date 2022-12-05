using Avalonia;
using System;

namespace PixelizerUI.ViewModels
{
    public class Observer : IObserver<AvaloniaPropertyChangedEventArgs<Size>>
    {
        private readonly MainWindowViewModel _vm;

        public Observer(MainWindowViewModel vm)
        {
            _vm = vm;
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(AvaloniaPropertyChangedEventArgs<Size> value)
        {
            _vm.Resize();
        }
    }
}
