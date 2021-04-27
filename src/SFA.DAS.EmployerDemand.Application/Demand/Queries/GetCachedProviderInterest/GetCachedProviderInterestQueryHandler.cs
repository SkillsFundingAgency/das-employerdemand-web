using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedProviderInterest
{
    public class GetCachedProviderInterestQueryHandler : IRequestHandler<GetCachedProviderInterestQuery, GetCachedProviderInterestQueryResult>
    {
        private readonly IDemandService _service;

        public GetCachedProviderInterestQueryHandler (IDemandService service)
        {
            _service = service;
        }
        public async Task<GetCachedProviderInterestQueryResult> Handle(GetCachedProviderInterestQuery request, CancellationToken cancellationToken)
        {
            var result = await _service.GetCachedProviderInterest(request.Id);

            return new GetCachedProviderInterestQueryResult
            {
                ProviderInterest = (ProviderInterestRequest) result
            };
        }
    }
}