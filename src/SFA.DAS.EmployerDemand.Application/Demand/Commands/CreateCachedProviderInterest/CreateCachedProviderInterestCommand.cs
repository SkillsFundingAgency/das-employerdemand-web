using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest
{
    public class CreateCachedProviderInterestCommand : IRequest<CreateCachedProviderInterestResult>
    {
        public IEnumerable<Guid> DemandIds { get; set; }
    }
}