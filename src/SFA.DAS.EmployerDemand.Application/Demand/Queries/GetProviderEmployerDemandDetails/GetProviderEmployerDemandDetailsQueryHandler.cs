using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using ProviderContactDetails = SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses.ProviderContactDetails;

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

            var result = await _demandService.GetProviderEmployerDemandDetails(request.Ukprn, request.CourseId, request.Location, request.LocationRadius);

            if (request.Id != null)
            {
                cacheResult = (ProviderInterestRequest)await _demandService.GetCachedProviderInterest((Guid)request.Id);

                result.ProviderContactDetails = new ProviderContactDetails
                {
                    Ukprn = cacheResult.Ukprn !=0 ? cacheResult.Ukprn : result.ProviderContactDetails.Ukprn,
                    EmailAddress = cacheResult.EmailAddress ?? result.ProviderContactDetails?.EmailAddress,
                    PhoneNumber = cacheResult.PhoneNumber ?? result.ProviderContactDetails?.PhoneNumber,
                    Website = cacheResult.Website ?? result.ProviderContactDetails?.Website
                };
            }

            return new GetProviderEmployerDemandDetailsQueryResult
            {
                Course = result.TrainingCourse,
                CourseDemandDetailsList = result.ProviderEmployerDemandDetailsList.Select(c => (ProviderCourseDemandDetails) c),
                SelectedLocation = result.Location,
                SelectedRadius = request.LocationRadius,
                ProviderContactDetails = result.ProviderContactDetails,
                EmployerDemandIds = !request.FromLocation ? cacheResult?.EmployerDemands?.Select(c => c.EmployerDemandId).ToList():new List<Guid>(),
                Id = cacheResult.Id
            };
        }
    }
}