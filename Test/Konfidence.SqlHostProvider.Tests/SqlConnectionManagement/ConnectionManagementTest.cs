using System;
using System.Linq;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.Tests.SqlConnectionManagement
{
    [TestClass]
    public class ConnectionManagementTest
    {
        [TestMethod]
        public void When_ConfigureSettings_read_with_multiple_connections_Should_set_them_all_in_ClientConfig()
        {
            // arrange
            var di = DependencyInjectionFactory.ConfigureDependencyInjection();

            // act
            var clientConfig = di.GetService<IClientConfig>();

            // assert
            if (!clientConfig.IsAssigned())
            {
                throw new Exception("mayor fail");
            }

            clientConfig.Connections.Should().HaveCountGreaterThan(1);
            clientConfig.Connections.Where(x => !x.Password.IsAssigned()).Should().HaveCount(0);
        }
    }
}
