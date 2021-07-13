using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Total_Commander.Model.Base;

namespace Total_Commander.Model
{
    public static class Logic
    {
        //Метод удаления выбранных файлов
        internal static void DeleteFiles(List<FileElement> selectedItems)
        {
            //Строка сообщения, которая будет показываться в MessageBox
            string message = string.Empty;

            if (selectedItems.Count == 1)
            {
                message = $"Вы уверены, что хотите переместить в Корзину выбранный файл {selectedItems[0].fileName + selectedItems[0].fileType}?";
            }
            else
            {
                string items = string.Empty;
                selectedItems.ForEach(x => items += "\n" + x.fileName + x.fileType);

                message = $"Вы уверены, что хотите переместить в Корзину выбранные файлы/каталоги ({selectedItems.Count} шт.)? {items}";            
            }

            if (MessageBox.Show(message, "Total Commander", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                selectedItems.ForEach(x =>
                {
                    try
                    {
                        //DeleteFile позволяет переместить файлы в корзину (или удалить их сразу, как и File.Delete)
                        FileSystem.DeleteFile(x.filePath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                    }
                    catch (System.Exception e)
                    {
                        MessageBox.Show($"{e.Message} для файла {x.filePath}", "Исключение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        //Метод перемещения выбранных файлов
        internal static void MoveFiles(List<FileElement> fileElements, string pathString)
        {
            fileElements.ForEach(x =>
            {
                try
                {
                    //Если в пути, куда нужно передвигать файлы, последний символ - не '\', то он добавляется.
                    if (pathString[pathString.Length - 1] != '\\')
                        //При передвижении файла нужно указывать его полное имя вторым параметром (откуда, куда)
                        File.Move(x.filePath, pathString + @"\" + x.fileName + x.fileType);
                    else
                        File.Move(x.filePath, pathString + x.fileName + x.fileType);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show($"{e.Message} для файла {x.filePath}", "Исключение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            MessageBox.Show("Действие выполнено.", "Total Commander", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        internal static void CopyFiles(List<FileElement> fileElements, string pathString)
        {
            fileElements.ForEach(x =>
            {
                try
                {
                    if (pathString[pathString.Length - 1] != '\\')
                        //При копировании файла тоже нужно указывать его полное имя вторым параметром (откуда, куда)
                        File.Copy(x.filePath, pathString + @"\" + x.fileName + x.fileType);
                    else
                        File.Copy(x.filePath, pathString + x.fileName + x.fileType);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show($"{e.Message} для файла {x.filePath}", "Исключение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            MessageBox.Show("Действие выполнено.", "Total Commander", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
