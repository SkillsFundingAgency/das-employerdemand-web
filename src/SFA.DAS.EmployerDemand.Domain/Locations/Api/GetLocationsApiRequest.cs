using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Locations.Api
{
    public class GetLocationsApiRequest : IGetApiRequest
    {
        private readonly string _searchTerm;

        public GetLocationsApiRequest(string searchTerm)
        {
            _searchTerm = searchTerm;
        }

        public string GetUrl => $"locations?searchTerm={_searchTerm}";
    }
}