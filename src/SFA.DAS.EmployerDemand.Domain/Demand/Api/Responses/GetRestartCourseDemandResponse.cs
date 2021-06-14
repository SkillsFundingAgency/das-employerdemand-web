using System;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class GetRestartCourseDemandResponse  
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }
        [JsonProperty("numberOfApprentices")]
        public int NumberOfApprentices { get; set; }
        [JsonProperty("trainingCourse")]
        public TrainingCourse Course { get; set; }
        [JsonProperty("location")]
        public LocationItem Location { get; set; }
        [JsonProperty("restartDemandExists")]
        public bool RestartDemandExists { get; set; }
        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }
        [JsonProperty("organisationName")]
        public string OrganisationName { get ; set ; }
    }
}