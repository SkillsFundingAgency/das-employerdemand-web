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

        public static Dictionary<string, int> BuildPropertyOrderDictionary()
        {
            var itemCount = 0;
            var propertyOrderDictionary = typeof(ProviderContactDetailsViewModel).GetProperties().Select(c => new
            {
                Order = itemCount++,
                c.Name
            }).ToDictionary(key => key.Name, value => value.Order);
            return propertyOrderDictionary;
        }


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
                EmployerDemands = source.EmployerDemands.Select(c => (EmployerDemands)c).ToList()
            };
        }
    }
}