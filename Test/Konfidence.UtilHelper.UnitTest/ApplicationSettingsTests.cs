using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.UtilHelper.UnitTest;

[TestClass]
public class ApplicationSettingsTests
{
    [TestMethod]
    public void Flush_CalledBeforeAnyGetOrSetStringValue_DoesNotThrow()
    {
        // Arrange
        TestContext context = CreateContext();

        try
        {
            // Act
            Action action = () => context.ApplicationSettings.Flush();

            // Assert
            action.Should().NotThrow();
        }
        finally
        {
            Directory.Delete(context.TempDirectory, true);
        }
    }

    private sealed class TestContext
    {
        public TestContext(ApplicationSettings applicationSettings, string tempDirectory)
        {
            ApplicationSettings = applicationSettings;
            TempDirectory = tempDirectory;
        }

        public ApplicationSettings ApplicationSettings { get; }

        public string TempDirectory { get; }
    }

    private static TestContext CreateContext()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), $"ApplicationSettingsTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDirectory);

        ApplicationSettings applicationSettings = new("testapp")
        {
            RootPath = tempDirectory + @"\"
        };

        return new TestContext(applicationSettings, tempDirectory);
    }
}
