using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ProviderContactDetailsViewModel
    {
        public Guid Id { get; set; }
        public int Ukprn { get ; set ; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public TrainingCourseViewModel Course { get; set; }
        public Dictionary<string,string> RouteDictionary { get ; set ; }

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
        
        public static implicit operator ProviderContactDetailsViewModel(ProviderInterestRequest source)
        {
            return new ProviderContactDetailsViewModel
            {
                Ukprn = source.Ukprn,
                Course = source.Course,
                Id = source.Id,
                Website = source.Website,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                RouteDictionary = new Dictionary<string, string>
                {
                    {"ukprn", source.Ukprn.ToString()},
                    {"id", source.Id.ToString()},
                    {"courseId", source.Course.Id.ToString()},
                }
            };
        }
    }
}