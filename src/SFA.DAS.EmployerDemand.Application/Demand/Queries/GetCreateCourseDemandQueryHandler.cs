using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries
{
    public class GetCreateCourseDemandQueryHandler : IRequestHandler<GetCreateCourseDemandQuery, GetCreateCourseDemandQueryResult>
    {
        private readonly IDemandService _demandService;

        public GetCreateCourseDemandQueryHandler (IDemandService demandService)
        {
            _demandService = demandService;
        }
        public async Task<GetCreateCourseDemandQueryResult> Handle(GetCreateCourseDemandQuery request, CancellationToken cancellationToken)
        {
            var course = await _demandService.GetCreateCourseDemand(request.TrainingCourseId);

            return new GetCreateCourseDemandQueryResult
            {
                TrainingCourse = course
            };
        }
    }
}