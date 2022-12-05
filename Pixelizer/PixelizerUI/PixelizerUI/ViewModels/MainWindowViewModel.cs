using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PixelizerUI.Views;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PixelizerUI.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _inputImage;

        [ObservableProperty]
        private string _outputPath;

        [ObservableProperty]
        private Bitmap _image;

        [ObservableProperty]
        private double _width;

        [ObservableProperty]
        private double _height;

        [ObservableProperty]
        private int _factor = 5;

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
                InputImage = result[0];
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

        partial void OnInputImageChanged(string value)
        {
            try
            {
                Image = new Bitmap(value);
                Resize();
            }
            catch (System.Exception)
            {
                Debug.WriteLine("Could create image!");
            }
        }

        [RelayCommand]
        public async Task Pixelize()
        {
            if (string.IsNullOrWhiteSpace(OutputPath) || string.IsNullOrWhiteSpace(InputImage))
            {
                return;
            }

            const string processPath = "./python-exe/main.exe";
            
            Process process = new()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.GetFullPath(processPath),
                    Arguments = $"\"{InputImage}\" \"{OutputPath}\" {Factor}",
                }
            };

            if (process.Start())
            {
                await process.WaitForExitAsync();
                Image = new Bitmap(OutputPath);
            }
        }

        public MainWindow MainWindow { get; set; }
    }
}
