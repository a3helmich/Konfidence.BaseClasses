using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.IntegrationTest;

[TestClass]
public class DependencyInjectionFactoryTests
{
    [TestMethod]
    public void GetApplicationPath_Always_ReturnsAppContextBaseDirectory()
    {
        // Arrange

        // Act
        string result = DependencyInjectionFactory.GetApplicationPath();

        // Assert
        result.Should().Be(AppContext.BaseDirectory);
    }
}
