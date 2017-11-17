using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Zabawki
{
    class File
    {
        string filePath;
        List<string> fileLines;

        public File()
        {
            fileLines = new List<string>();
        }

        public List<string> open(string path)
        {
            using(FileStream fs = System.IO.File.Open(path, FileMode.Open))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while(fs.Read(b,0,b.Length) > 0)
                {
                    fileLines.Add(temp.GetString(b));
                }
            }

            return fileLines;
        }

        public string getPathFromDialogWindow()
        {
            string initialDirectory = "H:\\";
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (Directory.Exists(initialDirectory))
            {
                fileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                fileDialog.InitialDirectory = "C:\\";
            }

            Nullable<bool> result = fileDialog.ShowDialog();
            if(result == true) return this.filePath = fileDialog.FileName;

            return "0;0;0;0;0;0;0";
        }


    }
}
