using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using PixelizerUI.ViewModels;
using PixelizerUI.Views;
using System;
using System.Threading.Tasks;

namespace PixelizerUI.Services
{
    public class DialogService
    {
        private readonly Window _mainWindow;

        public DialogService(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public async Task<T> ShowDialogAsync<T>(string title, Action<T> setParams = null) where T : class
        {
            DialogWindow dialogWindow = new();

            ServiceCollection services = new();
            services.AddSingleton(new DialogService(dialogWindow));
            services.AddSingleton(dialogWindow.NotificationManager);
            services.AddSingleton<T>();
            T dataContext = services.BuildServiceProvider().GetService<T>();

            setParams?.Invoke(dataContext);

            dialogWindow.DataContext = new DialogViewModel()
            {
                Title = title,
                DataContext = dataContext,
            };

            await dialogWindow.ShowDialog(_mainWindow);

            return dataContext;
        }
    }
}
