using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUnitTestClasses
{
    public class BaseFileTest
    {
        public static bool BitmapsEqual(string file1, string file2)
        {
            if (File.Exists(file1) && File.Exists(file2))
            {
                FileInfo file1Info = new FileInfo(file1);
                FileInfo file2Info = new FileInfo(file2);

                if (file1Info.Length == file2Info.Length)
                {
                    Assert.IsTrue(file1Info.Length < 100000, "Files zijn te groot");

                    byte[] file1ByteList = File.ReadAllBytes(file1);
                    byte[] file2ByteList = File.ReadAllBytes(file2);

                    int byteIndex = 0;
                    while (byteIndex < file1Info.Length)
                    {
                        if (file1ByteList[byteIndex] != file2ByteList[byteIndex])
                        {
                            return false;
                        }

                        byteIndex++;
                    }

                    return true;
                }

            }
            return false;
        }
    }
}
