using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure
{
    public static class ConfigBuilder
    {
        public static IConfigurationRoot GenerateConfiguration()
        {
            var configSource = new MemoryConfigurationSource
            {
                InitialData = new[]
                {
                    new KeyValuePair<string, string>("ConfigurationStorageConnectionString", "UseDevelopmentStorage=true;"),
                    new KeyValuePair<string, string>("ConfigNames", "SFA.DAS.EmployerDemand.Web"),
                    new KeyValuePair<string, string>("EnvironmentName", "DEV"),
                    new KeyValuePair<string, string>("Version", "1.0"),
                    new KeyValuePair<string, string>("StubProviderAuth", "true"),

                    new KeyValuePair<string, string>("EmployerDemandApi:Key", "test"),
                    new KeyValuePair<string, string>("EmployerDemandApi:BaseUrl", "http://localhost:5021/"),
                    new KeyValuePair<string, string>("EmployerDemandApi:PingUrl", "http://localhost:5021/"),
                    new KeyValuePair<string, string>("EmployerDemand:RedisConnectionString", ""),
                    new KeyValuePair<string, string>("ProviderIdams:MetadataAddress", ""),
                    new KeyValuePair<string, string>("ProviderIdams:Wtrealm", "https://localhost:5011/"),
                    new KeyValuePair<string, string>("ProviderSharedUIConfiguration:DashboardUrl", "https://at-pas.apprenticeships.education.gov.uk/"),
                }
            };

            var provider = new MemoryConfigurationProvider(configSource);

            return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
        }
    }
}

