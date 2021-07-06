using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Total_Commander.Model.Base;

namespace Total_Commander.CustomControl.Logic
{
    public static class TableLogic
    {
        internal static void GetFileElements(ObservableCollection<FileElement> fileElements, string pathString)
        {
            if (fileElements != null)
                fileElements.Clear();

            DirectoryInfo directoryInfo;
            foreach (string item in Directory.GetDirectories(pathString))
            {
                directoryInfo = new DirectoryInfo(item);
                fileElements.Add(new FileElement("[" + directoryInfo.Name + "]", directoryInfo.Extension, "<Папка>", directoryInfo.CreationTime.ToString(), "a"));
            }

            FileInfo fileInfo;
            foreach (string item in Directory.GetFiles(pathString))
            {
                fileInfo = new FileInfo(item);
                fileElements.Add(new FileElement(fileInfo.Name, fileInfo.Extension, fileInfo.Length.ToString(), fileInfo.CreationTime.ToString(), "a"));
            }
        }
    }
}
