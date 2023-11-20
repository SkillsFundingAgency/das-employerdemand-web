using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Infrastructure.Api;
using SFA.DAS.EmployerDemand.Infrastructure.UnitTests.HttpMessageHandlerMock;

namespace SFA.DAS.EmployerDemand.Infrastructure.UnitTests.Api
{
    public class WhenCallingPostOnTheApiClient
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            Guid responseId,
            PostData postContent,
            int id,
            EmployerDemandApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}/";
            var configMock = new Mock<IOptions<EmployerDemandApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                Content = new StringContent($"'{responseId}'"),
                StatusCode = HttpStatusCode.Accepted
            };
            var postTestRequest = new PostTestRequest(id) {Data = postContent};
            var expectedUrl = config.BaseUrl + postTestRequest.PostUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl, config.Key, HttpMethod.Post);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);
            
            
            //Act
            var actual = await apiClient.Post<string,PostData>(postTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Post)
                        && c.RequestUri.AbsoluteUri.Contains(postTestRequest.PostUrl)
                        && c.Content.ReadAsStringAsync().Result.Contains(postContent.Id)),
                    ItExpr.IsAny<CancellationToken>()
                );
            Guid.Parse(actual).Should().Be(responseId);
        }
        
        [Test, AutoData]
        public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
            PostData postContent,
            int id,
            EmployerDemandApi config)
        {
            //Arrange
            config.BaseUrl = $"https://{config.BaseUrl}/";
            var configMock = new Mock<IOptions<EmployerDemandApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            var postTestRequest = new PostTestRequest(id) {Data = postContent};
            var expectedUrl = config.BaseUrl + postTestRequest.PostUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl, config.Key, HttpMethod.Post);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);
            
            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => apiClient.Post<Guid, PostData>(postTestRequest));
            
        }
        
        private class PostTestRequest : IPostApiRequest<PostData>
        {
            private readonly int _id;

            public PostTestRequest (int id)
            {
                _id = id;
            }
            public PostData Data { get; set; }
            public string PostUrl => $"test-url/post/{_id}";
        }

        public class PostData
        {
            public string Id { get; set; }
        }
    }
}