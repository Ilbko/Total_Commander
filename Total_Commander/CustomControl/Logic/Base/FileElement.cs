using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Total_Commander.Model.Base
{
    public class FileElement
    {
        public string fileName { get; set; }
        public string fileType { get; set; }
        public string fileSize { get; set; }
        public string fileCreationDate { get; set; }
        public string fileAttributes { get; set; }

        public FileElement(string fileName, string fileType, string fileSize, string fileCreationDate, string fileAttributes)
        {
            this.fileName = fileName;
            this.fileType = fileType;
            this.fileSize = fileSize;
            this.fileCreationDate = fileCreationDate;
            this.fileAttributes = fileAttributes;
        }
    }
}
