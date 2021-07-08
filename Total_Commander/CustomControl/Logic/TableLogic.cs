using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Total_Commander.CustomControl.ViewModel;
using Total_Commander.Model.Base;

namespace Total_Commander.CustomControl.Logic
{
    public static class StringExtension
    {
        public static string TrimEndUntil(this string str, char symbol, bool removeSymbol)
        {
            int symbolIndex = str.LastIndexOf(symbol);
            string result = string.Empty;
            if (symbolIndex != -1)
            {
                if (removeSymbol)
                    result = str.Substring(0, symbolIndex);
                else
                    result = str.Substring(0, symbolIndex + 1);
            }
            else
                result = str;

            if (result.Contains(":") && !result.Contains("\\"))
                result = result.Insert(result.Length, "\\");

            return result;
        }
    }

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

            if (Directory.GetDirectoryRoot(pathString) != pathString)
            {
                App.Current.Dispatcher.Invoke(() => fileElements.Add(new FileElement("[..]", pathString.TrimEndUntil('\\', true), string.Empty, string.Empty, string.Empty, string.Empty)));
            }

            try
            {
                DirectoryInfo directoryInfo;
                foreach (string item in Directory.GetDirectories(pathString))
                {
                    directoryInfo = new DirectoryInfo(item);
                    App.Current.Dispatcher.Invoke(() => fileElements.Add(new FileElement("[" + directoryInfo.Name + "]", directoryInfo.FullName, directoryInfo.Extension, "<Папка>", directoryInfo.CreationTime.ToString(), directoryInfo.Attributes.ToString())));
                }

                FileInfo fileInfo;
                foreach (string item in Directory.GetFiles(pathString))
                {
                    fileInfo = new FileInfo(item);

                    //string fileName;
                    //int dotIndex = fileInfo.Name.IndexOf('.');
                    //fileName = dotIndex != -1 ? fileInfo.Name.Substring(0, dotIndex) : fileInfo.Name;

                    App.Current.Dispatcher.Invoke(() => fileElements.Add(new FileElement(fileInfo.Name.TrimEndUntil('.', true), fileInfo.FullName, fileInfo.Extension, fileInfo.Length.ToString(), fileInfo.CreationTime.ToString(), fileInfo.Attributes.ToString())));
                }
            } 
            catch (System.Exception e) 
            { 
                MessageBox.Show(e.Message, "Исключение", MessageBoxButton.OK, MessageBoxImage.Error); 
                return false; 
            }

            return true;
        }

        internal static void ModifyPathString(TextBox textBox, ref TableViewModel tableViewModel)
        {
            if (TableLogic.GetFileElements(tableViewModel.fileElements, textBox.Text))
                tableViewModel.PathString = textBox.Text;
            else
                tableViewModel.PathString = tableViewModel.PathString;

            Keyboard.ClearFocus();
        }

        internal static void DoubleClickItem(FileElement selectedItem, ref TableViewModel tableViewModel)
        {
            if (selectedItem == tableViewModel.SelectedElement)
            {
                if (Directory.Exists(tableViewModel.SelectedElement.filePath))
                {
                    tableViewModel.PathString = tableViewModel.SelectedElement.filePath;
                }
                else if (File.Exists(tableViewModel.SelectedElement.filePath))
                {
                    try
                    {
                        Process.Start(tableViewModel.SelectedElement.filePath);
                    }
                    catch (System.Exception e)
                    {
                        MessageBox.Show(e.Message, "Исключение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
