using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Scaffold.Desktop;

// Standard INotifyPropertyChanged implementation
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
