using System;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class UpdateProviderInterestDetails
    {
        [FromRoute]
        public int Ukprn { get; set; }
        [FromRoute]
        public int CourseId { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public string PhoneNumber { get; set; }
        public Guid Id { get; set; }
    }
}