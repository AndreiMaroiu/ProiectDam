using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PixelizerUI.Models;
using PixelizerUI.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

        public ObservableCollection<Notification> Notifications { get; } = new();

        public MainWindowViewModel()
        {
            TopLevel.ClientSizeProperty.Changed.Subscribe(new Observer(this));

            Reset();
        }

        [RelayCommand]
        public async Task OpenDialog()
        {
            OpenFileDialog dialog = new()
            {
                AllowMultiple = false,
                Title = "Input file location",
                Filters =
                {
                    new FileDialogFilter()
                    {
                        Extensions =
                        {
                            "png",
                            "jpg"
                        }
                    }
                }
            };

            string[] result = await dialog.ShowAsync(MainWindow);

            if (result is not null && result.Length is 1)
            {
                InputPath = result[0];
            }
        }

        [RelayCommand]
        public async Task ChooseOutput()
        {
            SaveFileDialog dialog = new()
            {
                Title = "Output location",
                DefaultExtension = ".png",
                InitialFileName = "output",
                Filters =
                {
                    new FileDialogFilter()
                    {
                        Extensions =
                        {
                            "png",
                            "jpg"
                        }
                    }
                }
            };

            string result = await dialog.ShowAsync(MainWindow);

            if (string.IsNullOrEmpty(result) is false)
            {
                OutputPath = result;
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

            Image = result;

            WasPixelized = true;
        }

        partial void OnIsComparingChanged(bool value)
        {
            if (!value)
            {
                WidthSliderValue = 0;
            }
        }

        public MainWindow MainWindow { get; set; }

        
        private unsafe Bitmap TryNativePixelize()
        {
            using MemoryStream memoryStream = new();
            InputImage.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            WriteableBitmap writeableBitmap = WriteableBitmap.Decode(memoryStream);
            using ILockedFramebuffer buffer = writeableBitmap.Lock();
            PixelFormat format = buffer.Format;
            Debug.Assert(format is PixelFormat.Bgra8888);

            PixelMatrix matrix = new(buffer.Address, buffer.Size);

            uint blockSize = (uint)Factor;

            for (uint row = 0, endRow = blockSize; row < Image.PixelSize.Height; row += blockSize, endRow += blockSize)
            {
                for (uint column = 0, endColumn = blockSize; column < Image.PixelSize.Width; column += blockSize, endColumn += blockSize)
                {
                    matrix.AverageColor(row, endRow, column, endColumn);
                }
            }

            writeableBitmap.Save(OutputPath);

            MainWindow.Manager.Show(new Notification("Image Pixelized!", "Image was pixelized with succes!", NotificationType.Success));

            return writeableBitmap;
        }

        [RelayCommand]
        public void Reset()
        {
            InputPath = string.Empty;
            OutputPath = string.Empty;
            InputImage = null;
            Image = null;
            Factor = 5;
            WasPixelized = false;
        }
    }
}
