using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnverifiedEmployerCourseDemand
{
    public class GetUnverifiedEmployerCourseDemandQueryHandler : IRequestHandler<GetUnverifiedEmployerCourseDemandQuery, GetUnverifiedEmployerCourseDemandQueryResult>
    {
        private readonly IDemandService _demandService;

        public GetUnverifiedEmployerCourseDemandQueryHandler (IDemandService demandService)
        {
            _demandService = demandService;
        }
        public async Task<GetUnverifiedEmployerCourseDemandQueryResult> Handle(GetUnverifiedEmployerCourseDemandQuery request, CancellationToken cancellationToken)
        {
            var result = await _demandService.GetUnverifiedEmployerCourseDemand(request.Id);

            return new GetUnverifiedEmployerCourseDemandQueryResult
            {
                CourseDemand = result
            };
        }
    }
}