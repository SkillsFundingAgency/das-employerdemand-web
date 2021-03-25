using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class LocationViewModel
    {
        public string Name { get; set; }
        
        public static implicit operator LocationViewModel(Location source)
        {
            return new LocationViewModel
            {
                Name = source.Name
            };
        }
    }
}