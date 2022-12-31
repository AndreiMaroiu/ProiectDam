using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PixelizerUI.Models;
using PixelizerUI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PixelizerUI.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private string _inputPath;
        [ObservableProperty] private string _outputPath;
        [ObservableProperty] private Bitmap _image;
        [ObservableProperty] private Bitmap _inputImage;
        [ObservableProperty] private double _width;
        [ObservableProperty] private double _height;
        [ObservableProperty] private int _factor = 5;
        [ObservableProperty] private bool _isComparing;
        [ObservableProperty] private bool _wasPixelized = false;
        [ObservableProperty] private int _widthSliderValue;

        private readonly IStorageProvider _storageProvider;
        private readonly INotificationManager _notificationManager;
        private readonly ClientSizeService _clientSizeService;

        private PixelizeResult _lastResult;

        public PixelizeSettingsViewModel PixelizeSettings { get; } = new();

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(IStorageProvider storageProvider, INotificationManager notificationManager, ClientSizeService clientSizeService) : this()
        {
            clientSizeService.OnSizeChanged += _ => Resize();

            _storageProvider = storageProvider;
            _notificationManager = notificationManager;
            _clientSizeService = clientSizeService;

            Reset();
        }

        [RelayCommand]
        public async Task OpenDialog()
        {
            var result = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Input file location",
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new FilePickerFileType("photos")
                    {
                        Patterns = new List<string>()
                        {
                            "*.png",
                            "*.jpg"
                        },
                    }
                },
            });

            if (result is not null && result.Count is 1)
            {
                result[0].TryGetUri(out var uri);
                InputPath = uri.LocalPath;
            }
        }

        [RelayCommand]
        public async Task ChooseOutput()
        {
            IStorageFile result = await _storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "Output location",
                DefaultExtension = ".png",
                SuggestedFileName = "output",
                ShowOverwritePrompt = true,
                FileTypeChoices = new List<FilePickerFileType>
                {
                    new FilePickerFileType("photos")
                    {
                        Patterns = new List<string>()
                        {
                            "*.png",
                            "*.jpg",
                        }
                    }
                }
            });

            if (result is not null && result.TryGetUri(out Uri uri))
            {
                OutputPath = uri.LocalPath;
            }
        }

        public void Resize()
        {
            if (Image is null)
            {
                return;
            }

            Size size = _clientSizeService.ClientSize;

            if (size.Height < Image.Size.Height)
            {
                double multiplier = (Image.Size.Height / size.Height);
                Width = Image.Size.Width / multiplier;
                Height = Image.Size.Height / multiplier;
            }
            else if (size.Height > Image.Size.Height)
            {
                double multiplier = (size.Height / Image.Size.Height);
                Width = Image.Size.Width * multiplier;
                Height = Image.Size.Height * multiplier;
            }
            else
            {
                Width = Image.Size.Width;
                Height = Image.Size.Height;
            }
        }

        partial void OnInputPathChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            try
            {
                Image = new Bitmap(value);
                Resize();
            }
            catch (Exception)
            {
                Debug.WriteLine("Could create image!");

                _notificationManager.Show(new Notification("Error", $"Path: {value} is not a valid image", NotificationType.Error));
            }
        }

        [RelayCommand]
        public async Task Pixelize()
        {
            if (string.IsNullOrWhiteSpace(OutputPath) || string.IsNullOrWhiteSpace(InputPath))
            {
                _notificationManager.Show(new Notification("Cannot pixelize", "Input or Output path are not valid", NotificationType.Error));
                return;
            }

            InputImage = new Bitmap(InputPath);

            var result = await Dispatcher.UIThread.InvokeAsync(TryNativePixelize);

            if (!result.IsSuccesul)
            {
                _notificationManager.Show(new Notification("Error!", "Image pixelization failed!", NotificationType.Error));
                return;
            }

            _notificationManager.Show(new Notification("Image Pixelized!", "Image was pixelized with succes!", NotificationType.Success));

            Image = result.UnscaledResult;
            WasPixelized = true;
            _lastResult = result;

            if (PixelizeSettings.AutoSave)
            {
                _ = Task.Run(() => result.Result.Save(OutputPath));
            }
        }

        partial void OnIsComparingChanged(bool value)
        {
            if (value)
            {
                WidthSliderValue = (int)Width / 2;
            }
            else
            {
                WidthSliderValue = 0;
            }
        }

        private async Task<PixelizeResult> TryNativePixelize()
        {
            try
            {
                Pixelizer algorithm = new(Factor);
                return await algorithm.PixelizeAsync(InputImage, PixelizeSettings.Strategy);
            }
            catch (Exception)
            {
                return PixelizeResult.Failed;
            }
        }

        [RelayCommand]
        private void Reset()
        {
            InputPath = string.Empty;
            OutputPath = string.Empty;
            InputImage = null;
            Image = null;
            Factor = 5;
            WasPixelized = false;
            _lastResult = null;
        }

        [RelayCommand]
        private void ClearOnlyOutput()
        {
            OutputPath = string.Empty;
            WasPixelized = false;

            if (InputImage is not null)
            {
                Image = InputImage;
                InputImage = null;
            }
        }

        [RelayCommand]
        private async Task PasteImage()
        {
            if (Application.Current!.Clipboard is { } clipboard)
            {
                object data = await clipboard.GetDataAsync(DataFormats.FileNames);

                if (data is List<string> fileNames && fileNames.Count > 0)
                {
                    Image = new Bitmap(fileNames[0]);
                    Resize();
                }
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            if (WasPixelized is false)
            {
                _notificationManager.Show(new Notification("Cannot save", "No pixelized image was created!", NotificationType.Error));
                return;
            }

            if (string.IsNullOrWhiteSpace(OutputPath))
            {
                await ChooseOutput();
            }

            _lastResult.Result.Save(OutputPath);
        }

        [RelayCommand]
        private async Task SaveAs()
        {
            if (WasPixelized is false)
            {
                _notificationManager.Show(new Notification("Cannot save", "No pixelized image was created!", NotificationType.Error));
                return;
            }

            await ChooseOutput();

            _lastResult.Result.Save(OutputPath);
        }
    }
}
