using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseRest.Client.UnitTest;

[TestClass]
public class RestClientConfigTests
{
    [TestMethod]
    public void Constructor_WithWebHostSection_BindsAllProperties()
    {
        // Arrange
        Dictionary<string, string?> settings = new()
        {
            ["WebHost:PortNr"] = "8080",
            ["WebHost:Address"] = "localhost",
            ["WebHost:BaseRoute"] = "api",
            ["WebHost:Route"] = "v1"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        // Act
        RestClientConfig clientConfig = new(configuration);

        // Assert
        clientConfig.PortNr.Should().Be(8080);
        clientConfig.Address.Should().Be("localhost");
        clientConfig.BaseRoute.Should().Be("api");
        clientConfig.Route.Should().Be("v1");
    }

    [TestMethod]
    public void Constructor_WithoutWebHostSection_LeavesDefaults()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder().Build();

        // Act
        RestClientConfig clientConfig = new(configuration);

        // Assert
        clientConfig.PortNr.Should().Be(0);
        clientConfig.Address.Should().BeEmpty();
        clientConfig.BaseRoute.Should().BeEmpty();
        clientConfig.Route.Should().BeEmpty();
    }

    [TestMethod]
    public void BaseUri_WithBoundValues_BuildsExpectedUri()
    {
        // Arrange
        Dictionary<string, string?> settings = new()
        {
            ["WebHost:PortNr"] = "8080",
            ["WebHost:Address"] = "localhost",
            ["WebHost:BaseRoute"] = "api",
            ["WebHost:Route"] = "v1"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        RestClientConfig clientConfig = new(configuration);

        // Act
        System.Uri result = clientConfig.BaseUri();

        // Assert
        result.Should().Be(new System.Uri("http://localhost:8080/api/v1"));
    }
}
