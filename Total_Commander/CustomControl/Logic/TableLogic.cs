using System.Collections.ObjectModel;
using System.IO;
using Total_Commander.Model.Base;

namespace Total_Commander.CustomControl.Logic
{
    public static class TableLogic
    {
        internal static bool GetFileElements(ObservableCollection<FileElement> fileElements, string pathString)
        {
            /*Приложения WPF запускаются с двумя важными потоками: потоком рендеринга и потоком UI. Коллекция fileElements,
             как и большинство объектов WPF, обрабатывается UI потоком. Если будет совершена попытка обработать коллекцию
             другим потоком, то будет вызвано исключение. App.Current.Dispatcher.Invoke позволяет исполнить команду от имени
             потока UI. (Нужно было имплементировать это после добавления FileSystemWatcher, который при срабатывании определённых событий
             вызывал метод GetFileElements).

             https://vkishorekumar.wordpress.com/2011/07/18/what-is-thread-affinity/
             */
            if (Directory.Exists(pathString))
            {
                if (fileElements != null)
                    App.Current.Dispatcher.Invoke(() => fileElements.Clear());
            }
            else 
                return false;

            DirectoryInfo directoryInfo;
            foreach (string item in Directory.GetDirectories(pathString))
            {
                directoryInfo = new DirectoryInfo(item);
                App.Current.Dispatcher.Invoke(() => fileElements.Add(new FileElement("[" + directoryInfo.Name + "]", directoryInfo.Extension, "<Папка>", directoryInfo.CreationTime.ToString(), directoryInfo.Attributes.ToString())));
            }

            FileInfo fileInfo;
            foreach (string item in Directory.GetFiles(pathString))
            {
                fileInfo = new FileInfo(item);

                string fileName;
                int dotIndex = fileInfo.Name.IndexOf('.');
                fileName = dotIndex != -1 ? fileInfo.Name.Substring(0, dotIndex) : fileInfo.Name;

                App.Current.Dispatcher.Invoke(() => fileElements.Add(new FileElement(fileName, fileInfo.Extension, fileInfo.Length.ToString(), fileInfo.CreationTime.ToString(), fileInfo.Attributes.ToString())));
            }

            return true;
        }
    }
}
