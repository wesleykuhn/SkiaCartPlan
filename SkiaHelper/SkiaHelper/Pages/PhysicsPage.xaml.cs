using SkiaHelper.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaHelper.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PhysicsPage : ContentPage
{
    public PhysicsPage()
    {
        InitializeComponent();

        BindingContext = new PhysicsViewModel(skglView);
    }
}