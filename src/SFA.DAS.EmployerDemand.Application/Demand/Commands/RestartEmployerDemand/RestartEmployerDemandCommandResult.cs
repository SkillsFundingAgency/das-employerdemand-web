using System;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.RestartEmployerDemand
{
    public class RestartEmployerDemandCommandResult
    {
        public Guid Id { get; set; }
        public bool RestartDemandExists { get; set; }
        public bool EmailVerified { get; set; }
        public int TrainingCourseId { get ; set ; }
        public DateTime? LastStartDate { get; set; }
        public string ContactEmail { get; set; }
    }
}