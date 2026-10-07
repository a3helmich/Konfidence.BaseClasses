using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PackageCleaner.UnitTest;

[TestClass]
public class KeepListTest
{
    private const string TwoPackages = """
        {
          "Keep": {
            "Konfidence.SqlHostProvider": [ 2021, 2022 ],
            "Konfidence.TestTools": [ 2024 ]
          }
        }
        """;

    [TestMethod]
    public void TryParse_WithEntries_KeepsOnlyTheListedPackageYears()
    {
        // Arrange
        // Act
        bool parsed = KeepList.TryParse(TwoPackages, out KeepList? keepList);

        // Assert
        parsed.Should().BeTrue();

        keepList!.Keeps(ParseFile("Konfidence.SqlHostProvider.2021.1.49.nupkg")).Should().BeTrue();
        keepList.Keeps(ParseFile("Konfidence.SqlHostProvider.2022.1.0.nupkg")).Should().BeTrue();
        keepList.Keeps(ParseFile("Konfidence.TestTools.2024.1.5.nupkg")).Should().BeTrue();

        keepList.Keeps(ParseFile("Konfidence.SqlHostProvider.2024.1.5.nupkg")).Should().BeFalse();
        keepList.Keeps(ParseFile("Konfidence.TestTools.2021.1.49.nupkg")).Should().BeFalse();
        keepList.Keeps(ParseFile("Konfidence.Mail.2021.1.49.nupkg")).Should().BeFalse();
    }

    [TestMethod]
    public void TryParse_WithPackageIdInAnotherCasing_StillKeeps()
    {
        // Arrange
        // Act
        KeepList.TryParse(TwoPackages, out KeepList? keepList);

        // Assert
        keepList!.Keeps(ParseFile("konfidence.sqlhostprovider.2021.1.49.nupkg")).Should().BeTrue();
    }

    [TestMethod]
    public void TryParse_WithSomethingThatIsNoJson_Fails()
    {
        // Arrange
        // Act
        bool parsed = KeepList.TryParse("not json at all", out KeepList? keepList);

        // Assert
        parsed.Should().BeFalse();

        keepList.Should().BeNull();
    }

    [TestMethod]
    public void Empty_KeepsNothing()
    {
        // Arrange
        // Act
        KeepList keepList = KeepList.Empty;

        // Assert
        keepList.Keeps(ParseFile("Konfidence.SqlHostProvider.2021.1.49.nupkg")).Should().BeFalse();
    }

    [TestMethod]
    public void GetEntriesMatchingNothing_ReportsEntriesWithoutAFileOnTheShare()
    {
        // Arrange
        KeepList.TryParse(TwoPackages, out KeepList? keepList);

        List<PackageFile> packageFiles =
        [
            ParseFile("Konfidence.SqlHostProvider.2021.1.49.nupkg"),
            ParseFile("Konfidence.SqlHostProvider.2026.5.6.nupkg")
        ];

        // Act
        List<string> stale = keepList!.GetEntriesMatchingNothing(packageFiles);

        // Assert
        stale.Should().BeEquivalentTo("Konfidence.SqlHostProvider 2022", "Konfidence.TestTools 2024");
    }

    private static PackageFile ParseFile(string fileName)
    {
        PackageFile.TryParse(fileName, out PackageFile? packageFile).Should().BeTrue();

        return packageFile!;
    }
}
