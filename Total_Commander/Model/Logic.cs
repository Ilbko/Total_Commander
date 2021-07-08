using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Total_Commander.Model.Base;

namespace Total_Commander.Model
{
    public static class Logic
    {
        internal static void DeleteFiles(List<FileElement> selectedItems)
        {
            if (selectedItems.Count == 0)
                MessageBox.Show("Не выбран ни один файл!", "Total Commander", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (selectedItems.Count == 1)
            {
                if (MessageBox.Show($"Вы уверены, что хотите переместить в Корзину выбранный файл {selectedItems[0].fileName + selectedItems[0].fileType}?", "Total Commander",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        FileSystem.DeleteFile(selectedItems[0].filePath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                    }
                    catch (System.Exception e)
                    {
                        MessageBox.Show(e.Message, "Исключение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                string items = string.Empty;
                selectedItems.ForEach(x => items += "\n" + x.fileName + x.fileType);

                if (MessageBox.Show($"Вы уверены, что хотите переместить в Корзину выбранные файлы/каталоги ({selectedItems.Count} шт.)? {items}", "Total Commander",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    selectedItems.ForEach(x =>
                    {
                        try
                        {
                            FileSystem.DeleteFile(x.filePath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                        } 
                        catch (System.Exception e)
                        {
                            MessageBox.Show(e.Message, "Исключение", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                }
            }
        }

        internal static void Move(List<FileElement> fileElements, string pathString)
        {
            fileElements.ForEach(x =>
            {
                try
                {
                    if (pathString[pathString.Length - 1] != '\\')
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

        internal static void Copy(List<FileElement> fileElements, string pathString)
        {
            fileElements.ForEach(x =>
            {
                try
                {
                    if (pathString[pathString.Length - 1] != '\\')
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
