using Xamarin.Forms;
using SkiaHelper.Pages;

namespace SkiaHelper;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new CartPlanPage();
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
}
