using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Requests;

namespace SFA.DAS.EmployerDemand.Application.Locations.Services
{
    public class LocationService : ILocationService
    {
        private readonly IApiClient _client;

        public LocationService (IApiClient client)
        {
            _client = client;
        }
        public async Task<Domain.Locations.Api.Responses.Locations> GetLocations(string searchTerm)
        {
            var request = new GetLocationsApiRequest(searchTerm);

            var result = await _client.Get<Domain.Locations.Api.Responses.Locations>(request);

            return result;
        }
    }
}