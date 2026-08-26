using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Konfidence.DatabaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.UnitTest;

[TestClass]
public class SqlHostProviderServiceCollectionExtensionsTests
{
    [TestMethod]
    public void AddSqlHostProviderServices_Always_RegistersTheDatabaseStructure()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Services.AddSqlHostProviderServices(context.Configuration);

        // Assert
        context.DescriptorFor<IDatabaseStructure>().ImplementationType.Should().Be<DatabaseStructure>();
    }

    [TestMethod]
    public void AddSqlHostProviderServices_Always_RegistersTheBaseClient()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Services.AddSqlHostProviderServices(context.Configuration);

        // Assert
        context.DescriptorFor<IBaseClient>().ImplementationType.Should().Be<SqlClient>();
    }

    [TestMethod]
    public void AddSqlHostProviderServices_Always_RegistersTheDataRepository()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Services.AddSqlHostProviderServices(context.Configuration);

        // Assert
        context.DescriptorFor<IDataRepository>().ImplementationType.Should().Be<SqlClientRepository>();
    }

    [TestMethod]
    public void AddSqlHostProviderServices_Always_RegistersTheClientConfigAsAnInstance()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Services.AddSqlHostProviderServices(context.Configuration);

        // Assert
        context.DescriptorFor<IClientConfig>().ImplementationInstance.Should().BeOfType<ClientConfig>();
    }

    [TestMethod]
    public void AddSqlHostProviderServices_Always_RegistersEverythingAsASingleton()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Services.AddSqlHostProviderServices(context.Configuration);

        // Assert
        context.Services.Should().OnlyContain(descriptor => descriptor.Lifetime == ServiceLifetime.Singleton);
    }

    [TestMethod]
    public void AddSqlHostProviderServices_Always_ReturnsTheSameCollectionSoItCanBeChained()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        IServiceCollection returned = context.Services.AddSqlHostProviderServices(context.Configuration);

        // Assert
        returned.Should().BeSameAs(context.Services);
    }

    [TestMethod]
    public void AddSqlHostProviderServices_WithADataConfigurationSection_BindsTheClientConfig()
    {
        // Arrange
        TestContext context = CreateContext(new Dictionary<string, string?>
        {
            ["DataConfiguration:DefaultDatabase"] = "Newsletter",
            ["DataConfiguration:ConfigFileFolder"] = @"C:\Projects\Newsletter"
        });

        // Act
        context.Services.AddSqlHostProviderServices(context.Configuration);

        // Assert
        ClientConfig clientConfig = context.ClientConfigInstance();

        clientConfig.DefaultDatabase.Should().Be("Newsletter");
        clientConfig.ConfigFileFolder.Should().Be(@"C:\Projects\Newsletter");
    }

    [TestMethod]
    public void AddSqlHostProviderServices_Always_RegistersOnlyTheFourClientServices()
    {
        // Arrange
        TestContext context = CreateContext();

        // Act
        context.Services.AddSqlHostProviderServices(context.Configuration);

        // Assert
        context.Services.Select(descriptor => descriptor.ServiceType).Should().BeEquivalentTo(
        [
            typeof(IDatabaseStructure),
            typeof(IBaseClient),
            typeof(IDataRepository),
            typeof(IClientConfig)
        ]);
    }

    [TestMethod]
    public void ConfigureDependencyInjection_Always_StillRegistersTheServiceCollectionItself()
    {
        // Arrange

        // Act
        IServiceProvider serviceProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

        // Assert
        serviceProvider.GetService<ServiceCollection>().Should().NotBeNull();
    }

    [TestMethod]
    public void ConfigureDependencyInjection_Always_StillResolvesTheClientConfig()
    {
        // Arrange

        // Act
        IServiceProvider serviceProvider = DependencyInjectionFactory.ConfigureDependencyInjection();

        // Assert
        serviceProvider.GetService<IClientConfig>().Should().NotBeNull();
    }

    private static TestContext CreateContext(Dictionary<string, string?>? settings = null)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings ?? [])
            .Build();

        return new TestContext(new ServiceCollection(), configuration);
    }

    private sealed class TestContext
    {
        public IServiceCollection Services { get; }

        public IConfiguration Configuration { get; }

        public TestContext(IServiceCollection services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
        }

        public ServiceDescriptor DescriptorFor<T>()
        {
            return Services.Single(descriptor => descriptor.ServiceType == typeof(T));
        }

        public ClientConfig ClientConfigInstance()
        {
            return (ClientConfig)DescriptorFor<IClientConfig>().ImplementationInstance!;
        }
    }
}
