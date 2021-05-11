using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ReviewProviderDetailsViewModel
    {
        public Guid Id { get; set; }
        public int Ukprn { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public TrainingCourseViewModel Course { get; set; }
        public List<EmployerDemands> EmployerDemands { get; set; }
        public Dictionary<string, string> RouteDictionary { get; set; }

        public static implicit operator ReviewProviderDetailsViewModel(ProviderInterestRequest source)
        {
            return new ReviewProviderDetailsViewModel
            {
                Ukprn = source.Ukprn,
                Course = source.Course,
                Id = source.Id,
                Website = source.Website,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                EmployerDemands = source.EmployerDemands.Select(c => (EmployerDemands)c).ToList(),
                RouteDictionary = new Dictionary<string, string>
                {
                    {"ukprn", source.Ukprn.ToString()},
                    {"id", source.Id.ToString()},
                    {"courseId", source.Course.Id.ToString()}
                }
            };
        }
    }
}