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
        public List<Guid> EmployerDemandIds { get; set; }
        public string Location { get; set; }
        public string Radius { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderWebsite { get; set; }
        public string ProviderTelephoneNumber { get; set; }
    }
}