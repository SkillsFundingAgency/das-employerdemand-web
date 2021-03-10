using System.Threading.Tasks;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface ILocationService
    {
        Task<Domain.Locations.Locations> GetLocations(string searchTerm);
    }
}