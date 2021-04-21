using System;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ProviderContactDetailsViewModel
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public TrainingCourseViewModel Course { get; set; }

        public static implicit operator ProviderContactDetailsViewModel(ProviderInterestRequest source)
        {
            return new ProviderContactDetailsViewModel
            {
                Course = source.Course,
                Id = source.Id,
                Website = source.Website,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber
            };
        }
    }
}