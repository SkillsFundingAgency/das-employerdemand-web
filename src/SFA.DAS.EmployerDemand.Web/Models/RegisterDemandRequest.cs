using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class RegisterDemandRequest
    {
        public int TrainingCourseId { get; set; }
        public string Location { get; set; }
        public bool? NumberOfApprenticesKnown { get; set; }
        public int? NumberOfApprentices { get; set; }
        public string OrganisationName { get; set; }
        public string ContactEmailAddress { get; set; }
     
        public static Dictionary<string, int> BuildPropertyOrderDictionary()
        {   
            var itemCount = 0;
            var propertyOrderDictionary = typeof(RegisterDemandRequest).GetProperties().Select(c => new
            {
                Order = itemCount++,
                c.Name
            }).ToDictionary(key => key.Name, value => value.Order);
            return propertyOrderDictionary;
        }
    }
    
}