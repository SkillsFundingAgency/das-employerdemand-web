using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedProviderInterest
{
    public class GetCachedProviderInterestQuery : IRequest<GetCachedProviderInterestQueryResult>
    {
        public Guid Id { get ; set ; }
    }
}