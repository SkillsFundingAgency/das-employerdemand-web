using System;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class StopEmployerCourseDemandResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }

        public static implicit operator StoppedCourseDemand(StopEmployerCourseDemandResponse source)
        {
            return new StoppedCourseDemand
            {
                Id = source.Id,
                ContactEmail = source.ContactEmail
            };
        }
    }
}