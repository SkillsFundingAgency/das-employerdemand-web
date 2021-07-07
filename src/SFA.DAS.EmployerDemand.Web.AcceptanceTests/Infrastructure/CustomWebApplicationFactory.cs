using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure;

namespace SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
                configurationBuilder.AddConfiguration(ConfigBuilder.GenerateConfiguration()));
        }
    }
}
