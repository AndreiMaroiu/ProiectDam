using CommunityToolkit.Mvvm.ComponentModel;
using PixelizerUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelizerUI.ViewModels
{
    public partial class PixelizeSettingsViewModel : ObservableObject
    {
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
    }
}
