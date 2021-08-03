using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using WireMock.Logging;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;

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
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/demand/[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}$"))
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("get-unverified-demand.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/demand/[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}/verify"))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("get-verified-demand.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/demand/(?!(?:bd11788e))[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}/restart"))
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("restart-demand.json"));

            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/demand/bd11788e-2480-4560-a76e-ffb99e582aa0/restart"))
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("restart-demand-expired-course.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/start/(?!(?:999))\\d+$"))
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("start-demand.json"));

            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/start/999$"))
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("start-demand-expired-course.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/create/(?!(?:999))\\d+$"))
                .UsingGet())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("create-demand.json"));

            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/create/999$"))
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("create-demand-expired-course.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/create"))
                    .WithParam(MatchLocationParam)
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("create-demand-location.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/demand/create"))
                .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithBody($"'{Guid.NewGuid().ToString()}'")
                );

            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/demand/[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}/stop"))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyFromFile("stop-demand.json")
                );
                
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/locations"))
                .UsingGet()).RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("locations.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/aggregated/providers/\\d+"))
                .UsingGet()).RespondWith(Response.Create()
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("provider-employer-demand.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/aggregated/providers/\\d+"))
                .WithParam(MatchLocationParam)
                .UsingGet()).RespondWith(Response.Create()
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("provider-employer-demand-location.json"));

            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/providers/\\d+/courses/\\d+"))
                .UsingGet()).RespondWith(Response.Create()
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("provider-employer-demand-details.json"));
            
            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, "/demand/providers/\\d+/courses/\\d+"))
                .WithParam(MatchLocationParam)
                .UsingGet()).RespondWith(Response.Create()
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("provider-employer-demand-details-location.json"));

            server.Given(Request.Create().WithPath(arg => Regex.IsMatch(arg, @"/providerinterest$"))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithBody($"'{Guid.NewGuid()}'")
                );

            return server;
        }
        
        private static bool MatchLocationParam(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("location") && arg["location"].Count !=0 && arg["location"][0].Equals("Camden, Camden");
        }
    }
}