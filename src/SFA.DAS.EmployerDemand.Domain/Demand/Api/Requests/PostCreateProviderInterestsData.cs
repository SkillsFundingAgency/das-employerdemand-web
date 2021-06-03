using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class PostCreateProviderInterestsData
    {
        public Guid Id { get ; set ; }
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
        public int Ukprn { get; set; }
        public string ProviderName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string FatUrl { get; set; }

        public PostCreateProviderInterestsData(IProviderDemandInterest interest, Func<IProviderDemandInterest, string> buildFatUrl)
        {
            Id = interest.Id;
            EmployerDemandIds = interest.EmployerDemands.Select(demands => demands.EmployerDemandId);
            Ukprn = interest.Ukprn;
            ProviderName = interest.ProviderName;
            Email = interest.EmailAddress;
            Phone = interest.PhoneNumber;
            Website = interest.Website;
            FatUrl = buildFatUrl(interest);
        }
    }
}