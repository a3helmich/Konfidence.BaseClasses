using FluentAssertions;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.Tests.SqlAccess;

[TestClass]
public class ClientConfigExtensionsTests
{
    [TestMethod]
    public void SetSqlApplicationSettings_With_no_DefaultDatabase_Should_return_without_touching_configuration()
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
