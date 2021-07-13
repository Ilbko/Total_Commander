using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Total_Commander.Model;
using Total_Commander.Model.Base;
using Total_Commander.View;
using Total_Commander.View.ViewModel;

namespace Total_Commander.ViewModel
{
    public class CommanderViewModel : INotifyPropertyChanged
    {
        //Список выбранных элементов (их передают кастомные элементы TableControl для их дальнейшей обработки)
        private List<FileElement> selectedItems = new List<FileElement>();
        public List<FileElement> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; OnPropertyChanged("SelectedItems"); }
        }

        //Команда копирования файлов
        private RelayCommand copyCommand;
        public RelayCommand CopyCommand
        {
            get
            {
                //При копировании и перемещении файлов создаётся новое окно через конструктор, где ему передаются список выбранных файлов и bool значение. От него зависит
                //текст в окне и команда, выполняемая при нажатии кнопки "ОК" (если true - копирование, false - перемещение)
                return copyCommand ?? new RelayCommand(act => 
                { 
                    if (this.SelectedItems.Count > 0)
                        new OperationWindow(this.SelectedItems, true).ShowDialog(); 
                    else
                        MessageBox.Show("Не выбран ни один файл!", "Total Commander", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        //Команда перемещения файлов
        private RelayCommand moveCommand;
        public RelayCommand MoveCommand
        {
            get
            {
                return moveCommand ?? new RelayCommand(act => 
                {
                    if (this.SelectedItems.Count > 0)
                        new OperationWindow(this.SelectedItems, false).ShowDialog();
                    else
                        MessageBox.Show("Не выбран ни один файл!", "Total Commander", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        //Команда удаления файлов
        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? new RelayCommand(act => 
                {
                    if (this.SelectedItems.Count > 0)
                        Logic.DeleteFiles(this.SelectedItems);
                    else
                        MessageBox.Show("Не выбран ни один файл!", "Total Commander", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        //Команда выхода с программы
        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ?? new RelayCommand(act => { Environment.Exit(0); });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
