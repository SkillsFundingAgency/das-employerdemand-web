namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class LocationViewModel
    {
        public string Name { get; set; }
        
        public static implicit operator LocationViewModel(Domain.Locations.LocationItem source)
        {
            return new LocationViewModel
            {
                Name = source.Name
            };
        }
    }
}