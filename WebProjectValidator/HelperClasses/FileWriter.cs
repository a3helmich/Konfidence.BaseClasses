using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.IO;

namespace WebProjectValidator.HelperClasses
{
    class FileWriter : BaseItem
    {
        public static void WriteLines(string fileName, List<string> newFileLines)
        {
            using (TextWriter textWriter = new StreamWriter(fileName, false, Encoding.Default))
            {
                foreach (string line in newFileLines)
                {
                    textWriter.WriteLine(line);
                }
            }
        }
    }
}
