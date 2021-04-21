using System;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ProviderContactDetailsViewModel
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
    }
}