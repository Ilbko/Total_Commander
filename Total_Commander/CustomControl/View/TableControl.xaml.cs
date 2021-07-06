using System.IO;
using System.Windows.Controls;
using Total_Commander.CustomControl.ViewModel;

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
    }
}
