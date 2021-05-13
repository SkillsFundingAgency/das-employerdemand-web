using System;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class VerifyEmployerCourseDemandResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }
        [JsonProperty("trainingCourse")]
        public TrainingCourse Course { get; set; }
        [JsonProperty("location")]
        public LocationItem Location { get; set; }
    }
}