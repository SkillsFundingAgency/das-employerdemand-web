using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.MockServer;
using SFA.DAS.EmployerDemand.Web;
using SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;
using WireMock.Server;

namespace SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure
{
    [Binding]
    public sealed class TestEnvironmentManagement
    {
        private readonly ScenarioContext _context;
        private static HttpClient _staticClient;
        private static IWireMockServer _staticApiServer;
        private Mock<IApiClient> _mockApiClient;
        private static TestServer _server;
        private CustomWebApplicationFactory<Startup> _webApp;

        public TestEnvironmentManagement(ScenarioContext context)
        {
            _context = context;
        }

        [BeforeScenario("WireMockServer")]
        public void StartWebApp()
        {
            _staticApiServer = MockApiServer.Start();
            _webApp = new CustomWebApplicationFactory<Startup>();
            
            _server = _webApp.Server;

            _staticClient = _server.CreateClient();
            _context.Set(_server, ContextKeys.TestServer);
            _context.Set(_staticClient,ContextKeys.HttpClient);
        }
        
        
        [BeforeScenario("MockApiClient")]
        public void StartWebAppWithMockApiClient()
        {
            _mockApiClient = new Mock<IApiClient>();

            _mockApiClient.Setup(x => x.Get<GetProviderEmployerDemandResponse>(It.IsAny<GetProviderEmployerDemandRequest>()))
                .ReturnsAsync(new GetProviderEmployerDemandResponse());

            _server = new TestServer(new WebHostBuilder()
                .ConfigureTestServices(services => ConfigureTestServices(services, _mockApiClient))
                .UseEnvironment(Environments.Development)
                .UseStartup<Startup>()
                .UseConfiguration(ConfigBuilder.GenerateConfiguration()));

            _staticClient = _server.CreateClient();
            
            _context.Set(_mockApiClient, ContextKeys.MockApiClient);
            _context.Set(_staticClient,ContextKeys.HttpClient);
        }

        [AfterScenario("WireMockServer")]
        public void StopEnvironment()
        {
            
            _webApp.Dispose();
            _server.Dispose();
            
            _staticApiServer?.Stop();
            _staticApiServer?.Dispose();
            _staticClient?.Dispose();
            
        }
        
        [AfterScenario("MockApiClient")]
        public void StopTestEnvironment()
        {
            _server.Dispose();
            _staticClient?.Dispose();
        }
        
        private void ConfigureTestServices(IServiceCollection serviceCollection,Mock<IApiClient> mockApiClient)
        {
            foreach(var descriptor in serviceCollection.Where(
                d => d.ServiceType ==
                     typeof(IApiClient)).ToList())
            {
                serviceCollection.Remove(descriptor);
            }
            serviceCollection.AddSingleton(mockApiClient);
            serviceCollection.AddSingleton(mockApiClient.Object);
        }
    }
}
