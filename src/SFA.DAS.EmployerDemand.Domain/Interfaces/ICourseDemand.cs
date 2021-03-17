using System;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Locations;

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
        LocationItem LocationItem { get; set; }
        TrainingCourse Course { get; set; }
    }
}