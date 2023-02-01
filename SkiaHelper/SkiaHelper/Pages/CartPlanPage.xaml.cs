using SkiaHelper.ViewModels;
using Xamarin.Forms;

namespace SkiaHelper.Pages;

public partial class CartPlanPage : ContentPage
{
    private readonly CartPlanViewModel _thisContext;

    public CartPlanPage()
    {
        InitializeComponent();

        _thisContext = new CartPlanViewModel(skglView);

        BindingContext = _thisContext;
    }
}
