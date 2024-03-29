using System;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class RestartCourseDemand
    {
        public Guid Id { get; set; }
        public bool RestartDemandExists { get; set; }
        public bool EmailVerified { get; set; }
        public int TrainingCourseId { get ; set ; }
        public DateTime? LastStartDate { get; set; }
        public string ContactEmail { get; set; }
    }
}