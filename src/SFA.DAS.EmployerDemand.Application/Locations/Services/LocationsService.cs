using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations.Api;

namespace SFA.DAS.EmployerDemand.Application.Locations.Services
{
    public class LocationService 
    {
        private readonly IApiClient _client;

        public LocationService (IApiClient client)
        {
            _client = client;
        }
        public async Task<Domain.Locations.Locations> GetLocations(string searchTerm)
        {
            var request = new GetLocationsApiRequest(searchTerm);

            var result = await _client.Get<Domain.Locations.Locations>(request);

            return result;
        }
    }
}