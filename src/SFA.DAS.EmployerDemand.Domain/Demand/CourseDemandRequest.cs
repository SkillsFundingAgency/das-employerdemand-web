using System;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class CourseDemandRequest : ICourseDemand
    {
        public Guid Id { get; set; }
        public int TrainingCourseId { get; set; }
        public string OrganisationName { get; set; }
        public string NumberOfApprentices { get; set; }
        public string Location { get; set; }
        public string ContactEmailAddress { get; set; }
        public LocationItem LocationItem { get; set; }
        public TrainingCourse Course { get; set; }
        public bool? NumberOfApprenticesKnown { get; set; }
    }
}