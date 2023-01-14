using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace PixelizerUI.ViewModels
{

    public partial class DialogViewModel : ObservableObject, IClose
    {
        [ObservableProperty] private string _title = "Title";
        [ObservableProperty] private object _dataContext;

        public bool CanClose => true;

        public event Action OnClose;

        [RelayCommand]
        private void Cancel()
        {
            OnClose?.Invoke();
        }

        [RelayCommand]
        private void Ok()
        {
            OnClose?.Invoke();
        }
    }
}
