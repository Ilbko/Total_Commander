using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Total_Commander.CustomControl.Logic;
using Total_Commander.CustomControl.ViewModel;
using Total_Commander.Model.Base;

namespace Total_Commander.View
{
    /// <summary>
    /// Логика взаимодействия для TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        public TableViewModel tableViewModel;

        //Событие нажатия на кнопку, когда фокус лежит на текстбоксе с путём директории
        private void pathTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TableLogic.ModifyPathString(sender as TextBox, ref this.tableViewModel);
        }

        //Событие двойного клика по элементу ЛистВью
        private void elementsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileElement selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as FileElement;

            TableLogic.DoubleClickItem(selectedItem, ref this.tableViewModel);
        }

        public TableControl()
        {
            InitializeComponent();
            this.DataContext = tableViewModel = new TableViewModel();
            //Наполнение КомбоБокса с дисками элементами
            foreach (var item in DriveInfo.GetDrives())
            {
                //С item убирается двоеточие, слеш и буква диска переводится в нижний регистр
                this.diskComboBox.Items.Add(item.Name.TrimEnd(':', '\\').ToLower());
            }

            //Выбирается первый элемент этого списка через привязанное свойство
            this.tableViewModel.SelectedDisk = DriveInfo.GetDrives()[0].Name.TrimEnd(':', '\\').ToLower();
        }

        //Свойство зависимости выбранных элементов. Нужно для передачи списка выбранных элементов к окну-родителю данного кастомного элемента.
        public static DependencyProperty selectedItemsProperty;
        public List<FileElement> SelectedItemsProperty
        {
            get { return (List<FileElement>)GetValue(selectedItemsProperty); }
            set { SetValue(selectedItemsProperty, value); }
        }

        static TableControl()
        {
            //Регистрация нового свойства зависимости
            selectedItemsProperty = DependencyProperty.Register("SelectedItemsProperty", typeof(List<FileElement>), typeof(TableControl),
                new FrameworkPropertyMetadata(new List<FileElement>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        }

        //Событие изменения выбранных элементов ЛистВью элементов
        private void elementsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SelectedItems ЛистВью хранит в IList. Сначала коллекцию нужно получить, а потом каждый элемент списка привести к нужному типу (а в конце привести коллекцию к списку)
            IList items = (sender as ListView).SelectedItems;
            SelectedItemsProperty = items.Cast<FileElement>().ToList();
        }
    }
}
