using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class GetProviderEmployerDemandResponse
    {
        [JsonProperty("employerDemand")]
        public IEnumerable<ProviderEmployerDemandItem> ProviderEmployerDemand { get; set; }
        
        [JsonProperty("trainingCourses")]
        public IEnumerable<TrainingCourse> TrainingCourses { get; set; }
        
        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }
        
        [JsonProperty("filteredResults")]
        public int FilteredResults { get; set; }
    }

    public class ProviderEmployerDemandItem
    {
        [JsonProperty("trainingCourse")]
        public TrainingCourse TrainingCourse { get; set; }
        [JsonProperty("apprentices")]
        public int Apprentices { get; set; }
        [JsonProperty("employers")]
        public int Employers { get; set; }
    }
}