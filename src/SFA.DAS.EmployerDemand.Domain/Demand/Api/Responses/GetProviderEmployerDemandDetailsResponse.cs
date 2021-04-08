using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class GetProviderEmployerDemandDetailsResponse
    {
        [JsonProperty("aggregatedCourseDemands")]
        public IEnumerable<ProviderEmployerDemandDetailsItem> ProviderEmployerDemandDetailsList { get; set; }
        [JsonProperty("trainingCourse")]
        public TrainingCourse TrainingCourse { get; set; }
        [JsonProperty("location")]
        public LocationItem Location { get; set; }
    }

    public class ProviderEmployerDemandDetailsItem
    {
        [JsonProperty("demandId")]
        public Guid DemandId { get; set; }
        [JsonProperty("apprenticesCount")]
        public int Apprentices { get; set; }
        [JsonProperty("location")]
        public LocationItem Location { get; set; }
    }
}