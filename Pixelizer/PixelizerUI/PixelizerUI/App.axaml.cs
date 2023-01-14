using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PixelizerUI.Services;
using PixelizerUI.ViewModels;
using PixelizerUI.Views;

namespace PixelizerUI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                MainWindow mainWindow = new();
                MainWindowViewModel viewModel = new(mainWindow.StorageProvider, mainWindow.Manager, new ClientSizeService(mainWindow), new DialogService(mainWindow));

                mainWindow.DataContext = viewModel;

                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}