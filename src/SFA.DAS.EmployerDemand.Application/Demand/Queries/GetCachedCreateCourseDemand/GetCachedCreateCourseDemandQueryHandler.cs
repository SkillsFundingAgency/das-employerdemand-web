using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand
{
    public class GetCachedCreateCourseDemandQueryHandler  : IRequestHandler<GetCachedCreateCourseDemandQuery, GetCachedCreateCourseDemandQueryResult>
    {
        private readonly IDemandService _demandService;

        public GetCachedCreateCourseDemandQueryHandler (IDemandService demandService)
        {
            _demandService = demandService;
        }
        
        public async Task<GetCachedCreateCourseDemandQueryResult> Handle(GetCachedCreateCourseDemandQuery request, CancellationToken cancellationToken)
        {
            var result = await _demandService.GetCachedCourseDemand(request.Id);

            return new GetCachedCreateCourseDemandQueryResult
            {
                CourseDemand = (CourseDemandRequest)result
            };
        }
    }
}