using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Total_Commander.View;
using Total_Commander.ViewModel;

namespace Total_Commander
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new CommanderViewModel();

            //Реализация этого кода уже есть в XAML.
            //ПРОБЛЕМА БЫЛА В ИСТОЧНИКЕ!
            //BindingOperations.SetBinding(this.firstFileTable, TableControl.selectedItemsProperty, new Binding()
            //{
            //    Source = this.commanderViewModel,
            //    Path = new PropertyPath("SelectedItems"),
            //    Mode = BindingMode.TwoWay,
            //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            //});
            //BindingOperations.SetBinding(this.secondFileTable, TableControl.selectedItemsProperty, new Binding()
            //{
            //    Source = this.commanderViewModel,
            //    Path = new PropertyPath("SelectedItems"),
            //    Mode = BindingMode.TwoWay,
            //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            //});
        }
    }
}
