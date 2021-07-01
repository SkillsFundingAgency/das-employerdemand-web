using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.MockServer;
using SFA.DAS.EmployerDemand.Web;
using SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;
using WireMock.Server;

namespace SFA.DAS.FAT.Web.AcceptanceTests.Infrastructure
{
    [Binding]
    public sealed class TestEnvironmentManagement
    {
        private readonly ScenarioContext _context;
        private static HttpClient _staticClient;
        private static IWireMockServer _staticApiServer;
        private Mock<IApiClient> _mockApiClient;
        private static TestServer _server;

        public TestEnvironmentManagement(ScenarioContext context)
        {
            _context = context;
        }

        [BeforeScenario("WireMockServer")]
        public void StartWebApp()
        {
            _staticApiServer = MockApiServer.Start();
            var webApp = new CustomWebApplicationFactory<Startup>();
            _server = webApp.Server;
            _staticClient = new CustomWebApplicationFactory<Startup>().CreateClient(new WebApplicationFactoryClientOptions{HandleCookies = false});
            _context.Set(_server, ContextKeys.TestServer);
            _context.Set(_staticClient,ContextKeys.HttpClient);
        }

        [AfterScenario("WireMockServer")]
        public void StopEnvironment()
        {
            _staticApiServer?.Stop();
            _staticClient?.Dispose();
        }

    }
}
