using System.IO;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.TestTools
{
    [UsedImplicitly]
    public static class FileCompareTool
    {
        [UsedImplicitly]
        public static bool BitmapEqual(string file1, string file2)
        {
            if (!File.Exists(file1) || !File.Exists(file2))
            {
                return false;
            }

            var file1Info = new FileInfo(file1);
            var file2Info = new FileInfo(file2);

            if (file1Info.Length != file2Info.Length)
            {
                return false;
            }

            Assert.IsTrue(file1Info.Length < 100000, "Files are too big");

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

        [UsedImplicitly]
        public static bool TextFileEqual(string file1, string file2)
        {
            if (!File.Exists(file1) || !File.Exists(file2))
            {
                return false;
            }

            var file1Info = new FileInfo(file1);
            var file2Info = new FileInfo(file2);

            if (file1Info.Length != file2Info.Length)
            {
                return false;
            }

            Assert.IsTrue(file1Info.Length < 100000, "Files are too big");

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
}
