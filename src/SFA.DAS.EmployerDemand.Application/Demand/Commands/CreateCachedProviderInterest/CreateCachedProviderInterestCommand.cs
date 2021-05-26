using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest
{
    public class CreateCachedProviderInterestCommand : IRequest<CreateCachedProviderInterestResult>, IProviderDemandInterest
    {
        public int Ukprn { get; set; }
        public IEnumerable<EmployerDemands> EmployerDemands { get; set; }
        public Guid Id { get; set; }
        public bool ProviderOffersThisCourse { get; set; }
        public string ProviderName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public Course Course { get; set; }
    }
}