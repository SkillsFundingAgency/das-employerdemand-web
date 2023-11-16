using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Infrastructure.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly EmployerDemandApi _config;

        public ApiClient (HttpClient httpClient, IOptions<EmployerDemandApi> config)
        {
            _httpClient = httpClient;
            _config = config.Value;
            _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        }
        
        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
            AddHeaders(httpRequestMessage);

            var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return default;
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TResponse>(json);    
            }
            
            response.EnsureSuccessStatusCode();
            
            return default;
        }

        public async Task<TResponse> Post<TResponse,TPostData>(IPostApiRequest<TPostData> request)
        {
            
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl)
            {
                Content = stringContent
            };
            AddHeaders(httpRequestMessage);
            
            var response = await _httpClient.SendAsync(httpRequestMessage)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);    
        }
        
        private void AddHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", _config.Key);
            request.Headers.Add("X-Version", "1");
        }
        
    }
}