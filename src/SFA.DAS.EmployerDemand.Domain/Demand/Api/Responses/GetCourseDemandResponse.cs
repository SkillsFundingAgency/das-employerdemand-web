using System;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class GetCourseDemandResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }
        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }
    }
}