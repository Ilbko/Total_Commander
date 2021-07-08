using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        private string pathString;
        public string PathString
        {
            get { return pathString; }
            set { pathString = value; OnPropertyChanged("PathString"); }
        }

        private RelayCommand<OperationWindow> okCommand;
        public RelayCommand<OperationWindow> OKCommand
        {
            get 
            {
                return okCommand ?? new RelayCommand<OperationWindow>(act => 
                {
                    if (Directory.Exists(PathString))
                    {
                        if (this.isCopy)
                            Logic.Copy(this.fileElements, this.PathString);
                        else
                            Logic.Move(this.fileElements, this.PathString);


                    }
                    else
                        MessageBox.Show("Директория не существует!", "Total Commander", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
        }

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
