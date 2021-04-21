using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IProviderDemandInterest
    {
        Guid Id { get; set; }
        int Ukprn { get; set; }
        IEnumerable<Guid> EmployerDemandIds { get; set; }
        string EmailAddress { get; set; }
        string PhoneNumber { get; set; }
        string Website { get; set; }
        int CourseId { get; set ; }
        int CourseLevel { get; set ; }
        string CourseTitle { get; set ; }
        string CourseSector { get; set ; }
    }
}