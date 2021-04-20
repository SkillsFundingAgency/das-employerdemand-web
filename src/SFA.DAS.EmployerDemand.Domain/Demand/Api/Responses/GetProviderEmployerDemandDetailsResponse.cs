using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class GetProviderEmployerDemandDetailsResponse
    {
        [JsonProperty("providerEmployerDemandDetailsList")]
        public IEnumerable<ProviderEmployerDemandDetailsItem> ProviderEmployerDemandDetailsList { get; set; }
        [JsonProperty("trainingCourse")]
        public TrainingCourse TrainingCourse { get; set; }
        [JsonProperty("location")]
        public LocationItem Location { get; set; }

        [JsonProperty("providerContactDetails")]
        public ProviderContactDetails ProviderContactDetails { get ; set ; }
    }

    public class ProviderEmployerDemandDetailsItem
    {
        [JsonProperty("id")]
        public Guid DemandId { get; set; }
        [JsonProperty("apprenticesCount")]
        public int Apprentices { get; set; }
        [JsonProperty("location")]
        public LocationItem Location { get; set; }
    }

    public class ProviderContactDetails
    {
        [JsonProperty("ukprn")]
        public int Ukprn { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("telephoneNumber")]
        public string TelephoneNumber { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
    }
}