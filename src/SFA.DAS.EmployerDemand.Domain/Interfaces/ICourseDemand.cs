using System;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface ICourseDemand
    {
        Guid Id { get ; set ; }
        int TrainingCourseId { get; set; }
        string OrganisationName { get; set; }
        string NumberOfApprentices { get; set; }
        string Location { get; set; }
        string ContactEmailAddress { get; set ; }
    }
}