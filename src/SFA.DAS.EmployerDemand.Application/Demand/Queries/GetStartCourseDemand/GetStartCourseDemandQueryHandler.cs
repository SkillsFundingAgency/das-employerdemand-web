using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand
{
    public class GetStartCourseDemandQueryHandler : IRequestHandler<GetStartCourseDemandQuery, GetStartCourseDemandQueryResult>
    {
        private readonly IDemandService _demandService;

        public GetStartCourseDemandQueryHandler (IDemandService demandService)
        {
            _demandService = demandService;
        }
        public async Task<GetStartCourseDemandQueryResult> Handle(GetStartCourseDemandQuery request, CancellationToken cancellationToken)
        {
            var result = await _demandService.GetStartCourseDemand(request.TrainingCourseId);

            return new GetStartCourseDemandQueryResult
            {
                Course = result.Course,
                EntryPoint = request.EntryPoint
            };
        }
    }
}