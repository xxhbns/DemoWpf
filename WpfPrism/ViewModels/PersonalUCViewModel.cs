using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfPrism.ViewModels
{
    public partial class PersonalUCViewModel : ObservableValidator
    {
        private readonly PaletteHelper _paletteHelper = new();

        public PersonalUCViewModel()
        {
        }

        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
                }
            }
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }


        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        [RelayCommand]
        private void ChangeHue(object? obj)
        {
            var color = (Color)obj!;

            ITheme theme = _paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }

    }
}
