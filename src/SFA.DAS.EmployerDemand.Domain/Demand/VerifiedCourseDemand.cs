using System;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class VerifiedCourseDemand
    {
        public Guid Id { get; set; }
        public string ContactEmail { get; set; }
        public Location Location { get; set; }
        public Course Course { get; set; }
        public int NumberOfApprentices { get ; set ; }

        public static implicit operator VerifiedCourseDemand(VerifyEmployerCourseDemandResponse source)
        {
            if (source == null)
            {
                return null;
            }
            return new VerifiedCourseDemand
            {
                Id = source.Id,
                Course = source.Course,
                Location = source.Location,
                ContactEmail = source.ContactEmail,
                NumberOfApprentices = source.NumberOfApprentices
            };
        }
    }
}