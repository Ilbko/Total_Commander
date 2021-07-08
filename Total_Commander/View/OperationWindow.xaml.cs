using System.Collections.Generic;
using System.Windows;
using Total_Commander.Model.Base;
using Total_Commander.ViewModel;

namespace Total_Commander.View
{
    /// <summary>
    /// Логика взаимодействия для OperationWindow.xaml
    /// </summary>
    public partial class OperationWindow : Window
    {
        public OperationWindow(List<FileElement> fileElements, bool isCopy)
        {
            InitializeComponent();
            this.DataContext = new OperationViewModel(fileElements, isCopy);

            if (isCopy)
                this.actionLabel.Content = $"Копировать файлы ({fileElements.Count} шт.) в:";
            else
                this.actionLabel.Content = $"Переместить файлы ({fileElements.Count} шт.) в:";
        }
    }
}
