using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using SFA.DAS.EmployerDemand.MockServer;
using TechTalk.SpecFlow;
using WireMock.Server;

namespace SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure
{
    [Binding]
    public sealed class TestEnvironmentManagement
    {
        private readonly ScenarioContext _context;
        private static HttpClient _staticClient;
        private static IWireMockServer _staticServer;

        public TestEnvironmentManagement(ScenarioContext context)
        {
            _context = context;
        }

        [BeforeTestRun]
        public static void StartEnvironment()
        {
            _staticServer = MockApiServer.Start();
            _staticClient = new WebApplicationFactory<Startup>().CreateClient();
        }

        [BeforeScenario]
        public void StartWebApp()
        {
            _context.Set(_staticClient, ContextKeys.HttpClient);
        }

        [AfterTestRun]
        public static void StopEnvironment()
        {
            _staticServer?.Stop();
            _staticServer?.Dispose();
        }
    }
}