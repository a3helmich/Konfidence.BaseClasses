using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Konfidence.BaseDependencyInjection;

public interface IKonfidenceDependencyInjection
{
    void AddServices(IServiceCollection services, IConfiguration configuration);
}
