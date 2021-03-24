
using TestClasses;

namespace TestByHandApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Dl.Test1DataItem();

            test.Save();
            test = new Dl.Test1DataItem();
            test.Save();
            test = new Dl.Test1DataItem();
            test.Save();
            test = new Dl.Test1DataItem();
            test.Save();
            test = new Dl.Test1DataItem();
            test.Save();

            var menuDataItems = Dl.Test1DataItem.GetList();

            menuDataItems.ForEach(menu => menu.Delete());
        }
    }
}
