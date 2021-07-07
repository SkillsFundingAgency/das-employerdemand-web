using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SFA.DAS.EmployerDemand.Web.AcceptanceTests.Steps
{
    [Binding]
    public sealed class HttpSteps
    {
        private readonly ScenarioContext _context;

        public HttpSteps(ScenarioContext context)
        {
            _context = context;
        }

        [When("I navigate to the following url: (.*)")]
        public async Task WhenINavigateToTheFollowingUrl(string url)
        {
            var client = _context.Get<HttpClient>(ContextKeys.HttpClient);
            var response = await client.GetAsync(url);
            _context.Set(response, ContextKeys.HttpResponse);
        }

        [When("I post to the following url: (.*)")]
        public async Task WhenIPostToTheFollowingUrl(string url, Table formBody)
        {
            var client = _context.Get<HttpClient>(ContextKeys.HttpClient);
            
            var contentDictionary = formBody.Rows.ToDictionary(r => r[0], r => r[1]);
            
            var content = new FormUrlEncodedContent(contentDictionary);
            
            var response = await client.PostAsync(url, content);
            _context.Set(response, ContextKeys.HttpResponse);
        }

        [Then("an http status code of (.*) is returned")]
        public void ThenAnHttpStatusCodeIsReturned(int httpStatusCode)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            result.StatusCode.Should().Be(httpStatusCode);
        }
        
    }
}

