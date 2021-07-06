using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Total_Commander.CustomControl.Logic;
using Total_Commander.Model.Base;

namespace Total_Commander.CustomControl.ViewModel
{
    public class TableViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FileElement> fileElements { get; set; }

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
                DiskSize = $"{driveInfo.AvailableFreeSpace / 1024} Кб из {driveInfo.TotalSize / 1024} Кб свободно";
                GC.Collect(GC.GetGeneration(driveInfo));

                this.PathString = this.SelectedDisk + @":\";
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

                TableLogic.GetFileElements(this.fileElements, this.PathString);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public TableViewModel()
        {
            this.fileElements = new ObservableCollection<FileElement>();
        }
    }
}
