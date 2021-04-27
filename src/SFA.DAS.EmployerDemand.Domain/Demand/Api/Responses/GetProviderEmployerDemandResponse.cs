using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class GetProviderEmployerDemandResponse
    {
        [JsonProperty("aggregatedCourseDemands")]
        public IEnumerable<ProviderEmployerDemandItem> ProviderEmployerDemand { get; set; }
        [JsonProperty("trainingCourses")]
        public IEnumerable<TrainingCourse> TrainingCourses { get; set; }
        [JsonProperty("total")]
        public int TotalResults { get; set; }
        [JsonProperty("totalFiltered")]
        public int FilteredResults { get; set; }
        [JsonProperty("location")]
        public LocationItem Location { get; set; }
        [JsonProperty("routes")]
        public List<RouteItem> Routes { get; set; }
    }

    public class ProviderEmployerDemandItem
    {
        [JsonProperty("trainingCourse")]
        public TrainingCourse TrainingCourse { get; set; }
        [JsonProperty("apprenticesCount")]
        public int Apprentices { get; set; }
        [JsonProperty("employersCount")]
        public int Employers { get; set; }
    }
    public class RouteItem
    {
        [JsonProperty("route")]
        public string Route { get; set; }
    }
}