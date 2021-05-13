using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCourseDemand
{
    public class GetCourseDemandQueryHandler : IRequestHandler<GetCourseDemandQuery, GetCourseDemandQueryResult>
    {
        private readonly IDemandService _demandService;

        public GetCourseDemandQueryHandler (IDemandService demandService)
        {
            _demandService = demandService;
        }
        public async Task<GetCourseDemandQueryResult> Handle(GetCourseDemandQuery request, CancellationToken cancellationToken)
        {
            var result = await _demandService.GetCourseDemand(request.Id);

            return new GetCourseDemandQueryResult
            {
                CourseDemand = result
            };
        }
    }
}