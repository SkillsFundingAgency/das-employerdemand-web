using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ProviderRegisterInterestRequest
    {
        [FromRoute]
        public int Ukprn { get; set; }
        [FromRoute]
        public int CourseId { get; set; }
        public List<Guid> EmployerCourseDemands { get; set; }
        public string Location { get; set; }
        public string Radius { get; set; }
    }
}