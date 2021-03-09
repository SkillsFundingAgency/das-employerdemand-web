using System.Text.RegularExpressions;
using WireMock.Logging;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.EmployerDemand.MockServer
{
    public static class MockApiServer
    {
        public static IWireMockServer Start()
        {
            var settings = new WireMockServerSettings
            {
                Port = 5021,
                Logger = new WireMockConsoleLogger()
            };

            var server = StandAloneApp.Start(settings);
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/create-demand"))
                .UsingGet()).RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("create-demand.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/locations"))
                .UsingGet()).RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("locations.json"));

            return server;
        }
    }
}