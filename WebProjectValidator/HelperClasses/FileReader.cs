using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.IO;

namespace WebProjectValidator.HelperClasses
{
    public class FileReader: BaseItem
    {
        public static List<string> ReadLines(string fileName)
        {
            List<string> fileLines = new List<string>();

            using (TextReader textReader = new StreamReader(fileName, Encoding.Default))
            {
                string line = textReader.ReadLine();

                while (IsAssigned(line))
                {
                    fileLines.Add(line);
                    line = textReader.ReadLine();
                }
            }

            return fileLines;
        }
    }
}
