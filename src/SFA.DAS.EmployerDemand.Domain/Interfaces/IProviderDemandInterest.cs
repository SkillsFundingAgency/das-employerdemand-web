using System;
using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IProviderDemandInterest
    {
        Guid Id { get; set; }
        int Ukprn { get; set; }
        IEnumerable<EmployerDemands> EmployerDemands { get; set; }
        string EmailAddress { get; set; }
        string PhoneNumber { get; set; }
        string Website { get; set; }
        public Course Course { get ; set ; }
    }
}