using SkiaHelper.ViewModelArgs;
using System.Numerics;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace SkiaHelper.ViewModels;

public class MathOperationsViewModel : BaseViewModel
{
    private AsyncCommand<string> mathOpButtonClickedCommand;
    public AsyncCommand<string> MathOpButtonClickedCommand =>
        mathOpButtonClickedCommand ??= new(MathOpButtonClickedCommandExecute, allowsMultipleExecutions: false);

    private readonly MathOperationArgs _receivedArgs;

    public MathOperationsViewModel(MathOperationArgs args)
    {
        _receivedArgs = args;
    }

    private async Task MathOpButtonClickedCommandExecute(string args)
    {
        switch (args)
        {
            case "Sub":
                _receivedArgs.Sub = true;
                break;

            case "Add":
                _receivedArgs.Add = true;
                break;

            case "Div":
                _receivedArgs.Div = true;
                break;

            case "Mult":
                _receivedArgs.Mult = true;
                break;
        }

        DoMathOperations();

        _receivedArgs.OnCompletedCallback.Invoke();

        await Shell.Current.Navigation.PopModalAsync();
    }

    private void DoMathOperations()
    {
        Vector2 result = _receivedArgs.Vectors[0].CartPlanPos;

        for (int i = 1; i < _receivedArgs.Vectors.Count; i++)
        {
            if (_receivedArgs.Sub)
                result = Vector2.Subtract(result, _receivedArgs.Vectors[i].CartPlanPos);

            else if (_receivedArgs.Add)
                result = Vector2.Add(result, _receivedArgs.Vectors[i].CartPlanPos);
        }

        _receivedArgs.ResultVec = result;
    }
}
