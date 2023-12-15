using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace SFA.DAS.EmployerDemand.Web.AppStart
{
    public static class AddDataProtectionExtension
    {
        public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            
            var fatWebConfig = configuration.GetSection(nameof(Domain.Configuration.EmployerDemand))
                .Get<Domain.Configuration.EmployerDemand>();

            if (fatWebConfig != null 
                && !string.IsNullOrEmpty(fatWebConfig.DataProtectionKeysDatabase) 
                && !string.IsNullOrEmpty(fatWebConfig.RedisConnectionString))
            {
                var redisConnectionString = fatWebConfig.RedisConnectionString;
                var dataProtectionKeysDatabase = fatWebConfig.DataProtectionKeysDatabase;

                var redis = ConnectionMultiplexer
                    .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

                services.AddDataProtection(options => { options.ApplicationDiscriminator = "das-provider";})
                    .SetApplicationName("das-provider")
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
            }
        }
    }
}