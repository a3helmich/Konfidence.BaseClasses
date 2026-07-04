using System.Collections.Generic;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.Tests.SqlAccess;

[TestClass]
public class ClientSettingsTests
{
    [TestMethod]
    public void Deserialize_Should_populate_UseEnvironmentSetting_and_Connections()
    {
        // Arrange
        const string json = """
            {
                "DataConfiguration": {
                    "UseEnvironmentSetting": true,
                    "DefaultDatabase": "TestClassGenerator",
                    "Connections": [
                        { "Server": "konfidence2", "Database": "TestClassGenerator", "ConnectionName": "TestClassGenerator" }
                    ]
                }
            }
            """;

        // Act
        json.Deserialize(out ClientSettings? clientSettings);

        // Assert
        clientSettings.Should().NotBeNull();
        clientSettings!.DataConfiguration.Should().NotBeNull();
        clientSettings.DataConfiguration!.UseEnvironmentSetting.Should().BeTrue();
        clientSettings.DataConfiguration.Connections.Should().BeEquivalentTo(new List<ConfigConnectionString>
        {
            new() { Server = "konfidence2", Database = "TestClassGenerator", ConnectionName = "TestClassGenerator" }
        });
    }
}
