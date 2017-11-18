using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Zabawki.Model;

namespace Zabawki
{
    class File
    {
        string filePath;
        List<string> fileLines;
        List<_3DCoordinates> coordinates;

        public File()
        {
            fileLines = new List<string>();
            coordinates = new List<_3DCoordinates>();
        }

        public List<string> open()
        {
            string [] readText = System.IO.File.ReadAllLines(filePath);

            foreach(string s in readText)
            {
                fileLines.Add(s);
            }

            return fileLines;
        }

        public string getPathFromDialogWindow()
        {
            string initialDirectory = @"E:\Google Drive\Uczelnia\Semestr 7\GUI WPF\Zabawki";
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
            if(result == true) return filePath = fileDialog.FileName;

            return "0;0;0;0;0;0;0";
        }

        public void read3DCoordinates()
        {
            foreach(var line in fileLines)
            {
                var exploded = line.Split(';');
                coordinates.Add( new _3DCoordinates( Int32.Parse(exploded[0]), Int32.Parse(exploded[1]), Int32.Parse(exploded[2]) ) );
            }
        }

        public List<_3DCoordinates> get3DCoordinates()
        {
            return coordinates;
        }

    }
}
