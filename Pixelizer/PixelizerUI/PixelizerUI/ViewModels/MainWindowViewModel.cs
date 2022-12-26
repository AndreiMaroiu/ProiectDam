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
    public partial class Notif : ObservableObject
    {
        [ObservableProperty]
        private Notification _content;

        [ObservableProperty]
        private bool _visibility;
    }

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
            try
            {
                Image = new Bitmap(value);
                Resize();
            }
            catch (System.Exception)
            {
                Debug.WriteLine("Could create image!");

                Notification notif = new("Error", $"Path: {value} is not a valid image", NotificationType.Error);
                Notifications.Add(notif);

                DispatcherTimer timer = new(TimeSpan.FromSeconds(2), DispatcherPriority.Normal, (sender, args) =>
                {
                    Notifications.Remove(notif);
                    (sender as DispatcherTimer).Stop();
                });

                timer.Start();
            }
        }

        [RelayCommand]
        public async Task Pixelize()
        {
            if (string.IsNullOrWhiteSpace(OutputPath) || string.IsNullOrWhiteSpace(InputPath))
            {
                return;
            }

            const string processPath = "./python-exe/main.exe";

            Process process = new()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.GetFullPath(processPath),
                    Arguments = $"\"{InputPath}\" \"{OutputPath}\" {Factor}",
                }
            };

            if (process.Start())
            {
                await process.WaitForExitAsync();
                Image = new Bitmap(OutputPath);
                WasPixelized = true;
            }
        }

        partial void OnIsComparingChanged(bool value)
        {
            if (value)
            {
                InputImage = new Bitmap(InputPath);
            }
            else
            {
                WidthSliderValue = 0;
            }
        }

        public MainWindow MainWindow { get; set; }

        [RelayCommand]
        public unsafe void TryNativePixelize()
        {
            using var memoryStream = new MemoryStream();
            Image.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var writeableBitmap = WriteableBitmap.Decode(memoryStream);
            using ILockedFramebuffer buffer = writeableBitmap.Lock();
            PixelFormat format = buffer.Format;
            Debug.Assert(format is PixelFormat.Bgra8888);

            PixelMatrix matrix = new(buffer.Address, buffer.Size);

            for (int i = 0; i < Image.PixelSize.Height; i++)
            {
                for (int j = 0; j < Image.PixelSize.Width; j++)
                {
                    int* pixelColor = matrix[i, j];

                    var color = NativeColor.FromInt(pixelColor);

                    *pixelColor = new NativeColor()
                    {
                        Red = color.Red,
                        Alpha = byte.MaxValue,
                    }.ToInt();
                }
            }

            //writeableBitmap.Save(OutputPath);

            Image = writeableBitmap;

            //using var memoryStream = new MemoryStream();
            //Image.Save(memoryStream);
            //memoryStream.Seek(0, SeekOrigin.Begin);
            //var writeableBitmap = WriteableBitmap.Decode(memoryStream);
            //using var lockedBitmap = writeableBitmap.Lock();

            //byte* bmpPtr = (byte*)lockedBitmap.Address;
            //int width = writeableBitmap.PixelSize.Width;
            //int height = writeableBitmap.PixelSize.Height;
            //byte* tempPtr;

            //for (int row = 0; row < height; row++)
            //{
            //    for (int col = 0; col < width; col++)
            //    {
            //        tempPtr = bmpPtr;
            //        byte red = *bmpPtr++;
            //        byte green = *bmpPtr++;
            //        byte blue = *bmpPtr++;
            //        byte alpha = *bmpPtr++;

            //        byte result = (byte)(0.2126 * red + 0.7152 * green + 0.0722 * blue);
            //        // byte result = (byte)((red + green + blue) / 3);

            //        bmpPtr = tempPtr;
            //        *bmpPtr++ = result; // red
            //        *bmpPtr++ = result; // green
            //        *bmpPtr++ = result; // blue
            //        *bmpPtr++ = alpha; // alpha
            //    }
            //}

            //Image = writeableBitmap;
        }
    }
}
