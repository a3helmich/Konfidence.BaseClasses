
using System.Collections.Generic;
using TestClasses;

namespace TestByHandApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Dl.Test1DataItem test = new();

            test.Save();
            test = new Dl.Test1DataItem();
            test.Save();
            test = new Dl.Test1DataItem();
            test.Save();
            test = new Dl.Test1DataItem();
            test.Save();
            test = new Dl.Test1DataItem();
            test.Save();

            List<Dl.Test1DataItem>? menuDataItems = Dl.Test1DataItem.GetList();

            menuDataItems.ForEach(menu => menu.Delete());
        }
    }
}
