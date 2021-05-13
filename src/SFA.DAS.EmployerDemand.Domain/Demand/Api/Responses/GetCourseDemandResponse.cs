using System;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class GetCourseDemandResponse
    {
        [JsonProperty("employerCourseDemand")]
        public EmployerCourseDemand EmployerCourseDemand { get; set; }
    }

    public class EmployerCourseDemand
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }
    }
}