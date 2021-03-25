using System.Threading.Tasks;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface ILocationService
    {
        Task<Locations.Api.Responses.Locations> GetLocations(string searchTerm);
    }
}