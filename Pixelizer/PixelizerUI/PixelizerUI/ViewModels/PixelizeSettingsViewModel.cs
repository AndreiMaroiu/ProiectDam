using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PixelizerUI.Models;
using PixelizerUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelizerUI.ViewModels
{
    public partial class PixelizeSettingsViewModel : ObservableObject
    {
        private readonly DialogService _dialogService;

        public PixelizeSettingsViewModel()
        {
            
        }

        public PixelizeSettingsViewModel(DialogService dialogService)
        {
            _dialogService = dialogService;
        }

        [ObservableProperty]
        private int _factor;

        [ObservableProperty]
        private bool _keepResolution;

        [ObservableProperty]
        private bool _showComparatorSlider;

        [ObservableProperty]
        private bool _autoSave = true;

        [ObservableProperty]
        private PixelizeStrategy _strategy = PixelizeStrategy.Average;

        public List<PixelizeStrategy> Strategies { get; } = Enum.GetValues(typeof(PixelizeStrategy)).Cast<PixelizeStrategy>().ToList();

        [RelayCommand]
        private void Reset()
        {
            KeepResolution = false;
            Strategy = PixelizeStrategy.Average;
            ShowComparatorSlider = false;
            AutoSave = true;
        }

        [RelayCommand]
        private async Task ShowInfo()
        {
            await _dialogService.ShowDialogAsync<PixelizeSettingsViewModel>("Info");
        }
    }
}
