using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Total_Commander.CustomControl.Logic;
using Total_Commander.CustomControl.ViewModel;
using Total_Commander.Model;

namespace Total_Commander.View
{
    /// <summary>
    /// Логика взаимодействия для TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        private TableViewModel tableViewModel;
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

        private void pathTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {               
                if (TableLogic.GetFileElements(this.tableViewModel.fileElements, (sender as TextBox).Text))
                    this.tableViewModel.PathString = (sender as TextBox).Text;
                else
                    this.tableViewModel.PathString = this.tableViewModel.PathString;

                Keyboard.ClearFocus();
            }
        }
    }
}
