using System;
using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class ProviderInterestRequest : IProviderDemandInterest
    {
        public Guid Id { get; set; }
        public int Ukprn { get; set; }
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public int CourseId { get; set; }
        public int CourseLevel { get; set; }
        public string CourseTitle { get; set; }
        public string CourseSector { get; set; }
    }
}