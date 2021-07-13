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
        //Наблюдаемая коллекция файлов(в какой-то директории) и смотритель за системой файлов для обновления списка в случае события в текущей директории
        public ObservableCollection<FileElement> fileElements { get; set; }
        private FileSystemWatcher fileWatcher = new FileSystemWatcher();

        //Выбранный элемент 
        private FileElement selectedElement;
        public FileElement SelectedElement
        {
            get { return selectedElement; }
            set { selectedElement = value; OnPropertyChanged("SelectedElement"); }
        }

        //Выбранный диск
        private string selectedDisk;
        public string SelectedDisk
        {
            get { return selectedDisk; }
            set 
            {
                //Выбранный диск берётся с Combobox, который наполняется буквами дисков.
                selectedDisk = value; 
                OnPropertyChanged("SelectedDisk");

                //Объект класса информации о диске
                DriveInfo driveInfo = new DriveInfo(this.SelectedDisk);
                try
                {
                    //Установка значения строки места на диске и установка строки пути к директории
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

        //Место на диске (свободное и полное)
        private string diskSize;
        public string DiskSize
        {
            get { return diskSize; }
            set { diskSize = value; OnPropertyChanged("DiskSize"); }
        }

        //Строка пути к директории
        private string pathString;
        public string PathString
        {
            get { return pathString; }
            set 
            { 
                pathString = value; 
                OnPropertyChanged("PathString");

                //При обновлении строки пути подгружаются элементы, находящиеся в директории по этому пути
                if (TableLogic.GetFileElements(this.fileElements, this.PathString))
                {
                    //Если удалось получить элементы в директории, то смотритель нацеливается на эту директорию
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

        //При каком-то событии в наблюдаемой директории вызывается обновление списка элементов (намного лучше, чем получать список элементов каждый промежуток времени)
        private void FileWatcher_Interacted(object sender, FileSystemEventArgs e)
        {
            TableLogic.GetFileElements(this.fileElements, this.PathString);
        }
    }
}
