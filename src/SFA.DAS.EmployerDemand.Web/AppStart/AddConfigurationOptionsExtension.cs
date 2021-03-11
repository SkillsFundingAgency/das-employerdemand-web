using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerDemand.Domain.Configuration;

namespace SFA.DAS.EmployerDemand.Web.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<EmployerDemandApi>(configuration.GetSection("EmployerDemandApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerDemandApi>>().Value);
            services.Configure<Domain.Configuration.EmployerDemand>(configuration.GetSection("EmployerDemand"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<Domain.Configuration.EmployerDemand>>().Value);
        }
    }
}