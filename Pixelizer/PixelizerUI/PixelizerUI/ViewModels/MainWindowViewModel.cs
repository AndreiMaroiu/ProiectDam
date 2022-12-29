using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PixelizerUI.Models;
using PixelizerUI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PixelizerUI.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _inputPath;

        [ObservableProperty]
        private string _outputPath;

        [ObservableProperty]
        private Bitmap _image;

        [ObservableProperty]
        private Bitmap _inputImage;

        [ObservableProperty]
        private double _width;

        [ObservableProperty]
        private double _height;

        [ObservableProperty]
        private int _factor = 5;

        [ObservableProperty]
        private bool _isComparing;

        [ObservableProperty]
        private bool _wasPixelized = false;

        [ObservableProperty]
        private int _widthSliderValue;

        public MainWindow MainWindow { get; set; }

        public MainWindowViewModel()
        {
            TopLevel.ClientSizeProperty.Changed.Subscribe(new Observer(this));

            Reset();
        }

        [RelayCommand]
        public async Task OpenDialog()
        {
            var result = await MainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
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
            IStorageFile result = await MainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
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

            if (MainWindow.ClientSize.Height < Image.Size.Height)
            {
                double multiplier = (Image.Size.Height / MainWindow.ClientSize.Height);
                Width = Image.Size.Width / multiplier;
                Height = Image.Size.Height / multiplier;
            }
            else if (MainWindow.ClientSize.Height > Image.Size.Height)
            {
                double multiplier = (MainWindow.ClientSize.Height / Image.Size.Height);
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

                MainWindow.Manager.Show(new Notification("Error", $"Path: {value} is not a valid image", NotificationType.Error));
            }
        }

        [RelayCommand]
        public async Task Pixelize()
        {
            if (string.IsNullOrWhiteSpace(OutputPath) || string.IsNullOrWhiteSpace(InputPath))
            {
                MainWindow.Manager.Show(new Notification("Cannot pixelize", "Input or Output path are not valid", NotificationType.Error));
                return;
            }

            InputImage = new Bitmap(InputPath);

            var result = await Dispatcher.UIThread.InvokeAsync(TryNativePixelize);

            if (!result.IsSuccesul)
            {
                MainWindow.Manager.Show(new Notification("Error!", "Image pixelization failed!", NotificationType.Error));
                return;
            }

            MainWindow.Manager.Show(new Notification("Image Pixelized!", "Image was pixelized with succes!", NotificationType.Success));

            Image = result.UnscaledResult;
            WasPixelized = true;

            _ = Task.Run(() => result.Result.Save(OutputPath));
        }

        partial void OnIsComparingChanged(bool value)
        {
            if (!value)
            {
                WidthSliderValue = 0;
            }
        }

        private async Task<PixelizeResult> TryNativePixelize()
        {
            try
            {
                var algorithm = new Pixelizer(Factor);
                return await algorithm.PixelizeAsync(InputImage);
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
    }
}
