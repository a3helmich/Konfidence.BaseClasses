using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.UtilHelper.UnitTest;

[TestClass]
public class BaseXmlDocumentTests
{
    private string _folder = string.Empty;

    [TestInitialize]
    public void Initialize()
    {
        _folder = Path.Combine(Path.GetTempPath(), "BaseXmlDocumentTests", Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(_folder);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, true);
        }
    }

    [TestMethod]
    public void Load_WithAFullPath_ReadsTheDocument()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.Load(context.FullPath);

        // Assert
        context.Document.Root.Should().NotBeNull();
    }

    [TestMethod]
    public void Load_WithAFullPath_KeepsTheFileNameItWasGiven()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.Load(context.FullPath);

        // Assert
        context.Document.FileName.Should().Be(context.FullPath);
    }

    [TestMethod]
    public void Load_WithAFullPath_ReportsTheFoldersOwnPath()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.Load(context.FullPath);

        // Assert
        context.Document.PathName.Should().Be(_folder + Path.DirectorySeparatorChar);
    }

    [TestMethod]
    public void Load_WithAFullPath_ReadsTheElementContent()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Document.Load(context.FullPath);

        // Assert
        context.Document.Root!.Name.Should().Be("PageSetting");
    }

    private TestContext CreateContext()
    {
        string fullPath = Path.Combine(_folder, "settings.xml");

        File.WriteAllText(fullPath, "<?xml version=\"1.0\" encoding=\"utf-8\" ?><PageSetting><Item>value</Item></PageSetting>");

        return new TestContext(new BaseXmlDocument(), fullPath);
    }

    private sealed class TestContext
    {
        public BaseXmlDocument Document { get; }

        public string FullPath { get; }

        public TestContext(BaseXmlDocument document, string fullPath)
        {
            Document = document;
            FullPath = fullPath;
        }
    }
}
