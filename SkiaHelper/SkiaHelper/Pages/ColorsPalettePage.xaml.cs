using SkiaHelper.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaHelper.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ColorsPalettePage : ContentPage
{
    public ColorsPalettePage()
    {
        InitializeComponent();

        BindingContext = new ColorsPaletteViewModel();
    }
}