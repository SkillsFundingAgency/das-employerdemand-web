using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Locations.Queries
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, GetLocationsQueryResult>
    {
        private readonly ILocationService _locationService;

        public GetLocationsQueryHandler (ILocationService locationService)
        {
            _locationService = locationService;
        }
        public async Task<GetLocationsQueryResult> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _locationService.GetLocations(request.SearchTerm);

            return new GetLocationsQueryResult
            {
                LocationItems = result.LocationItems
            };
        }
    }
}