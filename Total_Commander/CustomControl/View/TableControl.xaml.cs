using System;
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

        private void pathTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TableLogic.ModifyPathString(sender as TextBox, ref this.tableViewModel);
        }

        private void elementsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileElement selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as FileElement;

            TableLogic.DoubleClickItem(selectedItem, ref this.tableViewModel);
        }

        public TableControl()
        {
            InitializeComponent();
            this.DataContext = tableViewModel = new TableViewModel();
            foreach (var item in DriveInfo.GetDrives())
            {
                this.diskComboBox.Items.Add(item.Name.TrimEnd(':', '\\').ToLower());
            }

            this.tableViewModel.SelectedDisk = DriveInfo.GetDrives()[0].Name.TrimEnd(':', '\\').ToLower();
        }

        public static DependencyProperty selectedItemsProperty;
        public List<FileElement> SelectedItemsProperty
        {
            get { return (List<FileElement>)GetValue(selectedItemsProperty); }
            set { SetValue(selectedItemsProperty, value); }
        }

        static TableControl()
        {
            selectedItemsProperty = DependencyProperty.Register("SelectedItemsProperty", typeof(List<FileElement>), typeof(TableControl),
                new FrameworkPropertyMetadata(new List<FileElement>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        }

        private void elementsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IList items = (IList)(sender as ListView).SelectedItems;
            SelectedItemsProperty = items.Cast<FileElement>().ToList();
        }
    }
}
