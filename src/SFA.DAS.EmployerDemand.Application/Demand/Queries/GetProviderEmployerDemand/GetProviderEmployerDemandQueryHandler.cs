using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand
{
    public class GetProviderEmployerDemandQueryHandler : IRequestHandler<GetProviderEmployerDemandQuery, GetProviderEmployerDemandQueryResult>
    {
        private readonly IDemandService _demandService;

        public GetProviderEmployerDemandQueryHandler (IDemandService demandService)
        {
            _demandService = demandService;
        }
        public async Task<GetProviderEmployerDemandQueryResult> Handle(GetProviderEmployerDemandQuery request, CancellationToken cancellationToken)
        {
            if (request.CourseId != null)
            {
                request.SelectedRoutes = null;
            }
            
            var result = await _demandService.GetProviderEmployerDemand(request.Ukprn, request.CourseId, request.Location, request.LocationRadius, request.SelectedRoutes);

            return new GetProviderEmployerDemandQueryResult
            {
                Courses = result.TrainingCourses.Select(c => (Course) c),
                CourseDemands = result.ProviderEmployerDemand.Select(c => (ProviderCourseDemand) c),
                TotalFiltered = result.FilteredResults,
                TotalResults = result.TotalResults,
                SelectedCourseId = request.CourseId,
                SelectedLocation = result.Location,
                SelectedRadius = request.LocationRadius,
                Ukprn = request.Ukprn,
                Routes = result.Routes.Select(c => c.Route).ToList(),
                SelectedRoutes = request.SelectedRoutes
            };
        }
    }
}