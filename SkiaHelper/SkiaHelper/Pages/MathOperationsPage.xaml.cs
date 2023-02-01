using SkiaHelper.ViewModelArgs;
using SkiaHelper.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkiaHelper.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MathOperationsPage : ContentPage
{
    public MathOperationsPage(MathOperationArgs args)
    {
        InitializeComponent();

        BindingContext = new MathOperationsViewModel(args);
    }
}