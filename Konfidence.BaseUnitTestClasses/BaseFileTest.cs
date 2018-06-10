using System.IO;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUnitTestClasses
{
    [UsedImplicitly]
    public class BaseFileTest
    {
        [UsedImplicitly]
        public static bool BitmapsEqual(string file1, string file2)
        {
            if (File.Exists(file1) && File.Exists(file2))
            {
                var file1Info = new FileInfo(file1);
                var file2Info = new FileInfo(file2);

                if (file1Info.Length == file2Info.Length)
                {
                    Assert.IsTrue(file1Info.Length < 100000, "Files zijn te groot");

                    var file1ByteList = File.ReadAllBytes(file1);
                    var file2ByteList = File.ReadAllBytes(file2);

                    var byteIndex = 0;
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

        [UsedImplicitly]
        public static bool TextFileEqual(string file1, string file2)
        {
            if (File.Exists(file1) && File.Exists(file2))
            {
                var file1Info = new FileInfo(file1);
                var file2Info = new FileInfo(file2);

                if (file1Info.Length == file2Info.Length)
                {
                    Assert.IsTrue(file1Info.Length < 100000, "Files zijn te groot");

                    var file1ByteList = File.ReadAllBytes(file1);
                    var file2ByteList = File.ReadAllBytes(file2);

                    var byteIndex = 0;
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
