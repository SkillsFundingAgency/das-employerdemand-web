using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class RegisterDemandRequest
    {
        public Guid? CreateDemandId { get; set; }
        public int TrainingCourseId { get; set; }
        public string Location { get; set; }
        public bool? NumberOfApprenticesKnown { get; set; }
        public string NumberOfApprentices { get; set; }
        public string OrganisationName { get; set; }
        public string ContactEmailAddress { get; set; }
        public Guid? ExpiredCourseDemandId { get ; set ; }
        public short? EntryPoint { get; set; }

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