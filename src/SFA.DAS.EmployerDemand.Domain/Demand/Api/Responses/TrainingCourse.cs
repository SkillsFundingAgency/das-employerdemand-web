using System;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses
{
    public class TrainingCourse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }
        public DateTime? LastStartDate { get; set; }
    }
}