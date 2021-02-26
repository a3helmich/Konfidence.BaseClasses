using DbMenuClasses;

namespace TestByHandApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var menuDataItems = Bl.MenuDataItem.GetList();
        }
    }
}
