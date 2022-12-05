using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };

                (desktop.MainWindow.DataContext as MainWindowViewModel).MainWindow = desktop.MainWindow as MainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}