using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Total_Commander.Model.Base;
using Total_Commander.View.ViewModel;

namespace Total_Commander.ViewModel
{
    public class CommanderViewModel : INotifyPropertyChanged
    {
        private List<FileElement> selectedItems = new List<FileElement>();
        public List<FileElement> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; OnPropertyChanged("SelectedItems"); }
        }


        //private RelayCommand deleteCommand;
        //public RelayCommand DeleteCommand
        //{
        //    get
        //    {
        //        //return deleteCommand ?? new RelayCommand(() => );
        //    }
        //}
        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ?? new RelayCommand(act => { MessageBox.Show(SelectedItems.Count.ToString()); Environment.Exit(0); });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
