using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Total_Commander.CustomControl.ViewModel;
using Total_Commander.Model.Base;

namespace Total_Commander.CustomControl.Logic
{
    //Расширение класса строки
    public static class StringExtension
    {
        //Метод "Убирать символы с конца строки, пока не наткнулся на нужный символ"
        public static string TrimEndUntil(this string str, char symbol)
        {
            int symbolIndex = str.LastIndexOf(symbol);
            string result = string.Empty;

            //Если символ в строке был найден, то строка обрезается с конца (обрезается и символ)
            if (symbolIndex != -1)
                result = str.Substring(0, symbolIndex);           
            else
                result = str;

            //Если строка содержит двоеточие, но не содержит слеша, то значит, что строка - это только название диска с двоеточием
            //По такому пути нельзя перейти, поэтому в конце добавляется слеш.
            if (result.Contains(":") && !result.Contains("\\"))
                result = result.Insert(result.Length, "\\");

            return result;
        }
    }

    public static class TableLogic
    {
        //Метод получения элементов по пути директории
        internal static bool GetFileElements(ObservableCollection<FileElement> fileElements, string pathString)
        {
            /*Приложения WPF запускаются с двумя важными потоками: потоком рендеринга и потоком UI. Коллекция fileElements,
             как и большинство объектов WPF, обрабатывается UI потоком. Если будет совершена попытка обработать коллекцию
             другим потоком, то будет вызвано исключение. App.Current.Dispatcher.Invoke позволяет исполнить команду от имени
             потока UI. (Нужно было имплементировать это после добавления FileSystemWatcher, который при срабатывании определённых событий
             вызывал метод GetFileElements).

             https://vkishorekumar.wordpress.com/2011/07/18/what-is-thread-affinity/
             */
            //Если директория по заданному пути не существует, то возвращается false, что вызывает с главного окна MessageBox с информацией.
            if (!Directory.Exists(pathString))
                return false;

            if (fileElements != null)
                App.Current.Dispatcher.Invoke(() => fileElements.Clear());

            //Если строка пути - не просто название диска, то в начало коллекции добавляется "директория", позволяющая вернуться на шаг по пути
            if (Directory.GetDirectoryRoot(pathString) != pathString)
            {
                App.Current.Dispatcher.Invoke(() => fileElements.Add(new FileElement("[..]", pathString.TrimEndUntil('\\'), string.Empty, string.Empty, string.Empty, string.Empty)));
            }

            try
            {
                //Получение директорий, файлов и добавление их в коллекцию
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
                    App.Current.Dispatcher.Invoke(() => fileElements.Add(new FileElement(fileInfo.Name.TrimEndUntil('.'), fileInfo.FullName, fileInfo.Extension, fileInfo.Length.ToString(), fileInfo.CreationTime.ToString(), fileInfo.Attributes.ToString())));
                }
            } 
            catch (System.Exception e) 
            { 
                MessageBox.Show(e.Message, "Исключение", MessageBoxButton.OK, MessageBoxImage.Error); 
                return false; 
            }

            return true;
        }

        //Метод модификации строки пути (В поле для ввода пути директории можно вводить что угодно. Но при нажатии энтера начинается проверка - 
        //если путь существует, то строка модифицируется и список элементов обновляется, если не существует - то строка откатывается в изначальное положение.)
        internal static void ModifyPathString(TextBox textBox, ref TableViewModel tableViewModel)
        {
            //Если метод получения элементов не вернул false, то свойство, привязанное к текстбоксу равняется тексту в текстбоксе
            //(При изменении текстового поля привязанное поле изменяется тогда, когда с текстового поля уходит фокусировка, что позволяет
            //производить манипуляции ниже)
            if (TableLogic.GetFileElements(tableViewModel.fileElements, textBox.Text))
                tableViewModel.PathString = textBox.Text;
            //Если директории по пути не существует, то привязанное свойство равняется себе же, что обновляет привязанное текстовое поле
            else
                tableViewModel.PathString = tableViewModel.PathString;

            //Убирание фокусировки с поля ввода
            Keyboard.ClearFocus();
        }

        //Метод двойного клика по элементу
        internal static void DoubleClickItem(FileElement selectedItem, ref TableViewModel tableViewModel)
        {
            //Если элемент, по которому даблкликнули, равняется выбранному элементу (вместо проверки на null)
            if (selectedItem == tableViewModel.SelectedElement)
            {
                //Если элемент - директория
                if (Directory.Exists(tableViewModel.SelectedElement.filePath))
                {
                    //Обновляется свойство строки пути к директории (директория "открывается", обновляется список элементов)
                    tableViewModel.PathString = tableViewModel.SelectedElement.filePath;
                }
                //Если элемент - файл
                else if (File.Exists(tableViewModel.SelectedElement.filePath))
                {
                    //Попытка запустить этот файл
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
