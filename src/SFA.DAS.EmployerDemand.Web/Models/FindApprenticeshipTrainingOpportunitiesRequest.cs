using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class FindApprenticeshipTrainingOpportunitiesRequest
    {
        public int Ukprn { get; set; }
        [FromQuery] 
        public int? SelectedCourseId { get; set; }
        [FromQuery] 
        public string Location { get; set; }
        [FromQuery] 
        public string Radius { get; set; }
        [FromQuery] 
        public List<string> Routes { get; set; }
    }
}