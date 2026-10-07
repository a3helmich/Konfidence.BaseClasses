using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PackageCleaner.UnitTest;

[TestClass]
public class PackageCleanupManagerTest
{
    private const string PackageFolder = @"\\server\publish";
    private const string KeepFile = @"C:\keep\PackageCleanerKeepList.json";

    private const string KeepFileContent = """
        {
          "Keep": {
            "Konfidence.SqlHostProvider": [ 2021, 2022 ]
          }
        }
        """;

    [TestMethod]
    public void Constructor_WithoutPackageFolder_ExitsWith1()
    {
        // Arrange
        // Act
        TestContext context = CreateContext([$"--{Argument.Apply}=true"]);

        // Assert
        context.ErrorExiterMock.Verify(x => x.Exit(1), Times.Once);

        context.PackageCleanupManager.PackageFolder.Should().BeEmpty();
    }

    [TestMethod]
    public void Execute_WithoutPackageFolder_DoesNotTouchTheStore()
    {
        // Arrange
        TestContext context = CreateContext([]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.PackageFileStoreMock.Verify(x => x.GetPackageFileNames(It.IsAny<string>()), Times.Never);

        context.DeletedFileNames.Should().BeEmpty();
    }

    [TestMethod]
    public void Execute_WithNonExistingFolder_ExitsWith2()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs());

        context.PackageFileStoreMock.Setup(x => x.FolderExists(PackageFolder)).Returns(false);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.ErrorExiterMock.Verify(x => x.Exit(2), Times.Once);

        context.DeletedFileNames.Should().BeEmpty();
    }

    [TestMethod]
    public void Execute_WithSeveralYears_KeepsOnlyTheLatestYear()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs(), packageFileNames:
        [
            "Konfidence.Mail.2021.1.49.nupkg",
            "Konfidence.Mail.2022.1.0.nupkg",
            "Konfidence.Mail.2024.1.5.nupkg",
            "Konfidence.Mail.2026.5.6.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEquivalentTo(
            "Konfidence.Mail.2021.1.49.nupkg",
            "Konfidence.Mail.2022.1.0.nupkg",
            "Konfidence.Mail.2024.1.5.nupkg");
    }

    [TestMethod]
    public void Execute_WithSeveralVersionsInTheLatestYear_KeepsAllOfThem()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs(), packageFileNames:
        [
            "Konfidence.Mail.2024.1.5.nupkg",
            "Konfidence.Mail.2026.4.1.nupkg",
            "Konfidence.Mail.2026.5.6.nupkg",
            "Konfidence.Mail.2026.5.6.snupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEquivalentTo("Konfidence.Mail.2024.1.5.nupkg");
    }

    [TestMethod]
    public void Execute_WithOnePackageYearsBehindAnother_KeepsEachPackagesOwnLatestYear()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs(), packageFileNames:
        [
            "Konfidence.Mail.2022.1.0.nupkg",
            "Konfidence.Mail.2026.5.6.nupkg",
            "Konfidence.TestClasses.2021.1.49.nupkg",
            "Konfidence.TestClasses.2022.2.0.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEquivalentTo(
            "Konfidence.Mail.2022.1.0.nupkg",
            "Konfidence.TestClasses.2021.1.49.nupkg");
    }

    [TestMethod]
    public void Execute_WithAPackageFromOneYearOnly_RemovesNothingForIt()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs(), packageFileNames:
        [
            "WebLoggingClasses.2021.1.3.nupkg",
            "WebLoggingClasses.2021.1.2.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEmpty();
    }

    [TestMethod]
    public void Execute_RemovesSymbolPackagesAlongsideTheirPackage()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs(), packageFileNames:
        [
            "Konfidence.Mail.2024.1.5.nupkg",
            "Konfidence.Mail.2024.1.5.snupkg",
            "Konfidence.Mail.2026.5.6.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should()
            .BeEquivalentTo("Konfidence.Mail.2024.1.5.nupkg", "Konfidence.Mail.2024.1.5.snupkg");
    }

    [TestMethod]
    public void Execute_WithPackageIdsDifferingOnlyInCasing_TreatsThemAsOnePackage()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs(), packageFileNames:
        [
            "konfidence.mail.2024.1.5.nupkg",
            "Konfidence.Mail.2026.5.6.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEquivalentTo("konfidence.mail.2024.1.5.nupkg");
    }

    [TestMethod]
    public void Execute_WithFileNamesThatAreNoPackages_LeavesThoseAlone()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs(), packageFileNames:
        [
            "Konfidence.Mail.2024.1.5.nupkg",
            "Konfidence.Mail.2026.5.6.nupkg",
            "readme.txt",
            "Konfidence.Mail.2024.1.5.zip",
            "Konfidence.Mail.2024.1.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEquivalentTo("Konfidence.Mail.2024.1.5.nupkg");
    }

    [TestMethod]
    public void Execute_WithoutApply_RemovesNothing()
    {
        // Arrange
        TestContext context = CreateContext([$"--{Argument.PackageFolder}={PackageFolder}"], packageFileNames:
        [
            "Konfidence.Mail.2024.1.5.nupkg",
            "Konfidence.Mail.2026.5.6.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.PackageCleanupManager.Apply.Should().BeFalse();

        context.DeletedFileNames.Should().BeEmpty();
    }

    [TestMethod]
    public void Execute_WithApplyFalse_RemovesNothing()
    {
        // Arrange
        TestContext context = CreateContext(
            [$"--{Argument.PackageFolder}={PackageFolder}", $"--{Argument.Apply}=false"], packageFileNames:
            [
                "Konfidence.Mail.2024.1.5.nupkg",
                "Konfidence.Mail.2026.5.6.nupkg"
            ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.PackageCleanupManager.Apply.Should().BeFalse();

        context.DeletedFileNames.Should().BeEmpty();
    }

    [TestMethod]
    public void Execute_RemovesFromTheFolderItWasGiven()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgs(), packageFileNames:
        [
            "Konfidence.Mail.2024.1.5.nupkg",
            "Konfidence.Mail.2026.5.6.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.PackageFileStoreMock.Verify(x => x.Delete(PackageFolder, "Konfidence.Mail.2024.1.5.nupkg"), Times.Once);
    }

    [TestMethod]
    public void Execute_WithAKeepFile_KeepsTheListedPackageYears()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgsWithKeepFile(), packageFileNames:
        [
            "Konfidence.SqlHostProvider.2021.1.49.nupkg",
            "Konfidence.SqlHostProvider.2022.1.0.nupkg",
            "Konfidence.SqlHostProvider.2024.1.5.nupkg",
            "Konfidence.SqlHostProvider.2026.5.6.nupkg"
        ], keepFileContent: KeepFileContent);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEquivalentTo("Konfidence.SqlHostProvider.2024.1.5.nupkg");
    }

    [TestMethod]
    public void Execute_WithAKeepFileForAnotherPackage_RemovesTheOlderYearsAnyway()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgsWithKeepFile(), packageFileNames:
        [
            "Konfidence.Mail.2021.1.49.nupkg",
            "Konfidence.Mail.2026.5.6.nupkg"
        ], keepFileContent: KeepFileContent);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEquivalentTo("Konfidence.Mail.2021.1.49.nupkg");
    }

    [TestMethod]
    public void Execute_WithAKeepFileCoveringEveryOlderYear_RemovesNothing()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgsWithKeepFile(), packageFileNames:
        [
            "Konfidence.SqlHostProvider.2021.1.49.nupkg",
            "Konfidence.SqlHostProvider.2022.1.0.nupkg",
            "Konfidence.SqlHostProvider.2026.5.6.nupkg"
        ], keepFileContent: KeepFileContent);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.DeletedFileNames.Should().BeEmpty();
    }

    [TestMethod]
    public void Execute_WithAMissingKeepFile_ExitsWith3AndRemovesNothing()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgsWithKeepFile(), packageFileNames:
        [
            "Konfidence.SqlHostProvider.2021.1.49.nupkg",
            "Konfidence.SqlHostProvider.2026.5.6.nupkg"
        ]);

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.ErrorExiterMock.Verify(x => x.Exit(3), Times.Once);

        context.DeletedFileNames.Should().BeEmpty();
    }

    [TestMethod]
    public void Execute_WithAnUnreadableKeepFile_ExitsWith3AndRemovesNothing()
    {
        // Arrange
        TestContext context = CreateContext(ApplyingArgsWithKeepFile(), packageFileNames:
        [
            "Konfidence.SqlHostProvider.2021.1.49.nupkg",
            "Konfidence.SqlHostProvider.2026.5.6.nupkg"
        ], keepFileContent: "this is not json");

        // Act
        context.PackageCleanupManager.Execute();

        // Assert
        context.ErrorExiterMock.Verify(x => x.Exit(3), Times.Once);

        context.DeletedFileNames.Should().BeEmpty();
    }

    private static string[] ApplyingArgs()
    {
        return [$"--{Argument.PackageFolder}={PackageFolder}", $"--{Argument.Apply}=true"];
    }

    private static string[] ApplyingArgsWithKeepFile()
    {
        return [..ApplyingArgs(), $"--{Argument.KeepFile}={KeepFile}"];
    }

    private static TestContext CreateContext(string[] args, IReadOnlyList<string>? packageFileNames = null,
        string keepFileContent = "")
    {
        Mock<IPackageFileStore> packageFileStoreMock = new();
        Mock<IErrorExiter> errorExiterMock = new();

        List<string> deletedFileNames = [];

        packageFileStoreMock.Setup(x => x.FolderExists(PackageFolder)).Returns(true);
        packageFileStoreMock.Setup(x => x.FileExists(KeepFile)).Returns(keepFileContent.Length > 0);
        packageFileStoreMock.Setup(x => x.ReadAllText(KeepFile)).Returns(keepFileContent);
        packageFileStoreMock.Setup(x => x.GetPackageFileNames(PackageFolder)).Returns(packageFileNames ?? []);
        packageFileStoreMock.Setup(x => x.Delete(PackageFolder, It.IsAny<string>()))
            .Callback((string _, string fileName) => deletedFileNames.Add(fileName));

        PackageCleanupManager packageCleanupManager = new(args, packageFileStoreMock.Object, errorExiterMock.Object);

        return new TestContext(packageCleanupManager, packageFileStoreMock, errorExiterMock, deletedFileNames);
    }

    private sealed class TestContext
    {
        public PackageCleanupManager PackageCleanupManager { get; }

        public Mock<IPackageFileStore> PackageFileStoreMock { get; }

        public Mock<IErrorExiter> ErrorExiterMock { get; }

        public List<string> DeletedFileNames { get; }

        public TestContext(PackageCleanupManager packageCleanupManager, Mock<IPackageFileStore> packageFileStoreMock,
            Mock<IErrorExiter> errorExiterMock, List<string> deletedFileNames)
        {
            PackageCleanupManager = packageCleanupManager;
            PackageFileStoreMock = packageFileStoreMock;
            ErrorExiterMock = errorExiterMock;
            DeletedFileNames = deletedFileNames;
        }
    }
}
