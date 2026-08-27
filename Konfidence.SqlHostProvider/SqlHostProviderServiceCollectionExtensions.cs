using Konfidence.DatabaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Konfidence.SqlHostProvider;

public static class SqlHostProviderServiceCollectionExtensions
{
    public static IServiceCollection AddSqlHostProviderServices(this IServiceCollection services, IConfiguration configuration)
    {
        ClientConfig clientConfig = new(configuration);

        clientConfig.SetSqlApplicationSettings();

        return services
            .AddSingleton<IConnectionManagement, ConnectionManager>()
            .AddSingleton<IDatabaseStructure, DatabaseStructure>()
            .AddSingleton<IBaseClient, SqlClient>()
            .AddSingleton<IDataRepository, SqlClientRepository>()
            .AddSingleton<IClientConfig>(clientConfig);
    }
}
