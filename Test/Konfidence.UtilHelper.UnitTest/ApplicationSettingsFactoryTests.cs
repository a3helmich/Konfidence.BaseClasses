using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.UtilHelper.UnitTest;

[TestClass]
public class ApplicationSettingsFactoryTests
{
    [TestMethod]
    public void ApplicationSettings_ConcurrentCallsWithDifferentRootPaths_EachInstanceKeepsItsOwnRootPath()
    {
        // Arrange
        const int callCount = 50;
        string[] expectedRootPaths = new string[callCount];
        string[] actualRootPaths = new string[callCount];

        // Act
        Parallel.For(0, callCount, index =>
        {
            string rootPath = $@"C:\root{index}";
            expectedRootPaths[index] = rootPath + @"\settings\";

            ApplicationSettings applicationSettings =
                (ApplicationSettings)ApplicationSettingsFactory.ApplicationSettings("app", rootPath);

            actualRootPaths[index] = applicationSettings.RootPath;
        });

        // Assert
        // Before the fix, ApplicationSettingsFactory stashed the normalized root path in a shared
        // `private static string _rootPath` and read it back on the next line, so a concurrent call
        // with a different rootPath could overwrite it in between, leaking into this call's instance.
        actualRootPaths.Should().Equal(expectedRootPaths);
    }
}
