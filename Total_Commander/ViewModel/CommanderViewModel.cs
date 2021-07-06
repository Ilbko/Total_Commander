using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Total_Commander.Model.Base;

namespace Total_Commander.ViewModel
{
    public class CommanderViewModel : INotifyPropertyChanged
    {       
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
