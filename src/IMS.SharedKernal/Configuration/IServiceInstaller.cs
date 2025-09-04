using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.SharedKernal.Configuration;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}