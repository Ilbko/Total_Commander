namespace Total_Commander.Model.Base
{
    //Класс файла/директории
    public class FileElement
    {
        //Имя файла 
        public string fileName { get; set; }
        //Расширение файла
        public string fileType { get; set; }
        //Размер файла
        public string fileSize { get; set; }
        //Время создания файла
        public string fileCreationDate { get; set; }
        //Атрибуты файла
        public string fileAttributes { get; set; }
        //Путь к файлу
        public string filePath { get; set; }

        public FileElement(string fileName, string filePath, string fileType, string fileSize, string fileCreationDate, string fileAttributes)
        {
            this.fileName = fileName;
            this.filePath = filePath;
            this.fileType = fileType;
            this.fileSize = fileSize;
            this.fileCreationDate = fileCreationDate;
            this.fileAttributes = fileAttributes;
        }
    }
}
