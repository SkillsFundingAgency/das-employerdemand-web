using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCreateCourseDemand
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

            if (request.CreateDemandId.HasValue)
            {
                var cachedDemand = await _demandService.GetCachedCourseDemand(request.CreateDemandId.Value);

                if (cachedDemand != null)
                {
                    return new GetCreateCourseDemandQueryResult
                    {
                        CourseDemand = cachedDemand
                    };
                }
            }
            
            var createCourseDemandResponse = await _demandService.GetCreateCourseDemand(request.TrainingCourseId, "");

            return new GetCreateCourseDemandQueryResult
            {
                CourseDemand = new CourseDemandRequest
                {
                    Course = createCourseDemandResponse.Course 
                } 
            };
        }
    }
}