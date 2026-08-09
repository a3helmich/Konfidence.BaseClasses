using Konfidence.TestClasses.IntegrationTest.interfaces;

namespace Konfidence.TestClasses.IntegrationTest.objects;

public class TestItemClass : ITestItemClass
{
    public string Something { get; }

    public TestItemClass(string something)
    {
        Something = something;
    }

    public void MethodOne()
    {
    }

    public void MethodTwo()
    {
    }
}
