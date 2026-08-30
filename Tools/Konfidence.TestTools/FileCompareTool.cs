using System.IO;
using JetBrains.Annotations;

namespace Konfidence.TestTools;

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

        FileInfo file1Info = new(file1);
        FileInfo file2Info = new(file2);

        if (file1Info.Length != file2Info.Length)
        {
            return false;
        }

        if (file1Info.Length >= 10000)
        {
            return false;
        }

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

    [UsedImplicitly]
    public static bool TextFileEqual(string file1, string file2)
    {
        if (!File.Exists(file1) || !File.Exists(file2))
        {
            return false;
        }

        FileInfo file1Info = new(file1);
        FileInfo file2Info = new(file2);

        if (file1Info.Length != file2Info.Length)
        {
            return false;
        }

        if (file1Info.Length >= 10000)
        {
            return false;
        }

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
