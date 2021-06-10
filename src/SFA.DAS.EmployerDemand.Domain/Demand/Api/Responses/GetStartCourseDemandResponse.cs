using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class GetStartCourseDemandResponse
    {
        [JsonProperty("trainingCourse")]
        public TrainingCourse Course { get; set; }
    }
}