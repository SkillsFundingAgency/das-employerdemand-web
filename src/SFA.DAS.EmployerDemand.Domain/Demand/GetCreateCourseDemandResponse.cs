using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class GetCreateCourseDemandResponse
    {
        [JsonProperty("trainingCourse")]
        public TrainingCourse Course { get; set; }
    }
}