using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Total_Commander.CustomControl.Logic;
using Total_Commander.Model.Base;

namespace Total_Commander.CustomControl.ViewModel
{
    public class TableViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FileElement> fileElements { get; set; }
        private FileSystemWatcher fileWatcher = new FileSystemWatcher();

        private FileElement selectedElement;
        public FileElement SelectedElement
        {
            get { return selectedElement; }
            set { selectedElement = value; OnPropertyChanged("SelectedElement"); }
        }

        private string selectedDisk;
        public string SelectedDisk
        {
            get { return selectedDisk; }
            set 
            { 
                selectedDisk = value; 
                OnPropertyChanged("SelectedDisk");

                DriveInfo driveInfo = new DriveInfo(selectedDisk);
                try
                {
                    this.DiskSize = $"{driveInfo.AvailableFreeSpace / 1024} Кб из {driveInfo.TotalSize / 1024} Кб свободно";
                    this.PathString = this.SelectedDisk + @":\";
                } 
                catch (System.Exception e)
                {
                    MessageBox.Show(e.Message, "Исключение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                GC.Collect(GC.GetGeneration(driveInfo));
            }
        }

        private string diskSize;
        public string DiskSize
        {
            get { return diskSize; }
            set { diskSize = value; OnPropertyChanged("DiskSize"); }
        }

        private string pathString;
        public string PathString
        {
            get { return pathString; }
            set 
            { 
                pathString = value; 
                OnPropertyChanged("PathString");

                if (TableLogic.GetFileElements(this.fileElements, this.PathString))
                {
                    this.fileWatcher.Path = PathString;
                    this.fileWatcher.EnableRaisingEvents = true;
                }
                else
                    this.fileWatcher.EnableRaisingEvents = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public TableViewModel()
        {
            this.fileElements = new ObservableCollection<FileElement>();

            this.fileWatcher.Changed += FileWatcher_Interacted;
            this.fileWatcher.Created += FileWatcher_Interacted;
            this.fileWatcher.Deleted += FileWatcher_Interacted;
            this.fileWatcher.Renamed += FileWatcher_Interacted;

            //Перед установкой значения этого поля класса FileSystemWatcher должен иметь наблюдаемый путь.
            //this.fileWatcher.EnableRaisingEvents = true;
        }

        private void FileWatcher_Interacted(object sender, FileSystemEventArgs e)
        {
            TableLogic.GetFileElements(this.fileElements, this.PathString);
        }
    }
}
