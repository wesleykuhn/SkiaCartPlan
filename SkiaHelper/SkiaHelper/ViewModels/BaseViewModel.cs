using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SkiaHelper.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    #region [ PROP WATCHER ]

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;

        backingStore = value;

        OnPropertyChanged(propertyName);

        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        var changed = PropertyChanged;

        changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    public Task DisplayAlert(string title, string message, string cancel) =>
        MainThread.IsMainThread ?
        Application.Current.MainPage.DisplayAlert(title, message, cancel) :
        MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.DisplayAlert(title, message, cancel));
}
