using FluentAssertions;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.IntegrationTest.SqlAccess;

[TestClass]
public class ClientConfigExtensionsTests
{
    [TestMethod]
    public void SetSqlApplicationSettings_WithNoDefaultDatabase_ReturnsWithoutTouchingConfiguration()
    {
        // Arrange
        IConfigurationRoot configuration = new ConfigurationBuilder().Build();
        ClientConfig clientConfig = new(configuration);

        // Act
        clientConfig.SetSqlApplicationSettings();

        // Assert
        clientConfig.DefaultDatabase.Should().BeEmpty();
    }
}
