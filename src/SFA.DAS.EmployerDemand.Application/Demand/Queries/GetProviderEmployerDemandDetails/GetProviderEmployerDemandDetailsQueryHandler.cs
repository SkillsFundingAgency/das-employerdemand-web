using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails
{
    public class GetProviderEmployerDemandDetailsQueryHandler : IRequestHandler<GetProviderEmployerDemandDetailsQuery, GetProviderEmployerDemandDetailsQueryResult>
    {
        private readonly IDemandService _demandService;

        public GetProviderEmployerDemandDetailsQueryHandler (IDemandService demandService)
        {
            _demandService = demandService;
        }
        public async Task<GetProviderEmployerDemandDetailsQueryResult> Handle(GetProviderEmployerDemandDetailsQuery request, CancellationToken cancellationToken)
        {
            var cacheResult = new ProviderInterestRequest();
            
            if (request.Id != null)
            {
                cacheResult = (ProviderInterestRequest)await _demandService.GetCachedProviderInterest((Guid)request.Id);
            }

            var result = await _demandService.GetProviderEmployerDemandDetails(request.Ukprn, request.CourseId, request.Location, request.LocationRadius);

            return new GetProviderEmployerDemandDetailsQueryResult
            {
                Course = result.TrainingCourse,
                CourseDemandDetailsList = result.ProviderEmployerDemandDetailsList.Select(c => (ProviderCourseDemandDetails) c),
                SelectedLocation = result.Location,
                SelectedRadius = request.LocationRadius,
                ProviderContactDetails = result.ProviderContactDetails,
                ProviderName = result.ProviderName,
                EmployerDemandIds = cacheResult?.EmployerDemands?.Select(c => c.EmployerDemandId).ToList(),
                Id = cacheResult?.Id ?? Guid.Empty
            };
        }
    }
}