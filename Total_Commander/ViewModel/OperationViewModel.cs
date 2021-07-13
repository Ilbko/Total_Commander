using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Total_Commander.Model;
using Total_Commander.Model.Base;
using Total_Commander.View;

namespace Total_Commander.ViewModel
{
    public class OperationViewModel : INotifyPropertyChanged
    {
        private bool isCopy;
        private List<FileElement> fileElements;

        //Путь, по которому будет производиться действие (копирование, перемещение)
        private string pathString;
        public string PathString
        {
            get { return pathString; }
            set { pathString = value; OnPropertyChanged("PathString"); }
        }

        //Команда нажатия на кнопку "ОК"
        private RelayCommand<OperationWindow> okCommand;
        public RelayCommand<OperationWindow> OKCommand
        {
            get 
            {
                return okCommand ?? new RelayCommand<OperationWindow>(act => 
                {
                    if (Directory.Exists(PathString))
                    {
                        //Если в конструкторе вторым параметром было передано true, то будет произведено копирование. Перемещение файлов будет в обратном случае.
                        if (this.isCopy)
                            Logic.CopyFiles(this.fileElements, this.PathString);
                        else
                            Logic.MoveFiles(this.fileElements, this.PathString);

                        act.Close();
                    }
                    else
                        MessageBox.Show("Директория не существует!", "Total Commander", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
        }

        //Команда нажатия на кнопку "Отмена"
        private RelayCommand<OperationWindow> cancelCommand;
        public RelayCommand<OperationWindow> CancelCommand
        {
            get
            {
                return cancelCommand ?? new RelayCommand<OperationWindow>(act => act.Close());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public OperationViewModel(List<FileElement> fileElements, bool isCopy)
        {
            this.fileElements = fileElements;
            this.isCopy = isCopy;
        }
    }
}
