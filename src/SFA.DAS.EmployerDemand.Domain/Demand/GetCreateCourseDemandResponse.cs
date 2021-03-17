using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class GetCreateCourseDemandResponse
    {
        [JsonProperty("trainingCourse")]
        public TrainingCourse Course { get; set; }
        [JsonProperty("location")]
        public Locations.LocationItem Location { get; set; }
    }
}