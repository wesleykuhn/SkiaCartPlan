using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
}
