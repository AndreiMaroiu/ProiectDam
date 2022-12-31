using Avalonia;
using Avalonia.Controls;
using PixelizerUI.Models;
using PixelizerUI.ViewModels;
using System;

namespace PixelizerUI.Services
{
    public class ClientSizeService
    {
        private readonly Window _window;

        public ClientSizeService(Window window)
        {
            _window = window;
            TopLevel.ClientSizeProperty.Changed.Subscribe(new ClientSizeObserver(window, value => OnSizeChanged?.Invoke(value)));
        }

        public Size ClientSize => _window.ClientSize;

        public event Action<Size> OnSizeChanged;
    }
}
