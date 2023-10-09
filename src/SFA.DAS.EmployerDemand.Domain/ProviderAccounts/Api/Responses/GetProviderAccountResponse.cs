using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerDemand.Domain.ProviderAccounts.Api.Responses
{
    public class GetProviderAccountResponse
    {
        [JsonPropertyName("canAccessService")]
        public bool CanAccessService { get; set; }
    }
}