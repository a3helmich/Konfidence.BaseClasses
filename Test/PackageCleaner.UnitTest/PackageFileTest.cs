using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PackageCleaner.UnitTest;

[TestClass]
public class PackageFileTest
{
    [TestMethod]
    [DataRow("Konfidence.Mail.2026.4.1.nupkg", "Konfidence.Mail", 2026)]
    [DataRow("Konfidence.Mail.2026.4.1.snupkg", "Konfidence.Mail", 2026)]
    [DataRow("ClientSettingsUpdater.2021.1.49.nupkg", "ClientSettingsUpdater", 2021)]
    [DataRow("Konfidence.Project-References.2021.1.1.nupkg", "Konfidence.Project-References", 2021)]
    [DataRow("Konfidence.DatabaseInterfaceXXX.2022.1.0.nupkg", "Konfidence.DatabaseInterfaceXXX", 2022)]
    [DataRow("Konfidence.Mail.2026.4.1.NUPKG", "Konfidence.Mail", 2026)]
    public void TryParse_WithAPackageFileName_YieldsThePackageIdAndYear(string fileName, string expectedPackageId,
        int expectedYear)
    {
        // Arrange
        // Act
        bool parsed = PackageFile.TryParse(fileName, out PackageFile? packageFile);

        // Assert
        parsed.Should().BeTrue();

        packageFile!.FileName.Should().Be(fileName);
        packageFile.PackageId.Should().Be(expectedPackageId);
        packageFile.Year.Should().Be(expectedYear);
    }

    [TestMethod]
    [DataRow("readme.txt")]
    [DataRow("Konfidence.Mail.2026.4.1.zip")]
    [DataRow("Konfidence.Mail.2026.4.nupkg")]
    [DataRow("Konfidence.Mail.nupkg")]
    [DataRow("2026.4.1.nupkg")]
    [DataRow("Konfidence.Mail.202.4.1.nupkg")]
    public void TryParse_WithSomethingElse_YieldsNothing(string fileName)
    {
        // Arrange
        // Act
        bool parsed = PackageFile.TryParse(fileName, out PackageFile? packageFile);

        // Assert
        parsed.Should().BeFalse();

        packageFile.Should().BeNull();
    }
}
