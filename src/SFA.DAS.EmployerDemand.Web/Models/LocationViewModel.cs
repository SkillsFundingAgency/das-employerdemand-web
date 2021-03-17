using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class LocationViewModel
    {
        public string Name { get; set; }
        
        public static implicit operator LocationViewModel(LocationItem source)
        {
            return new LocationViewModel
            {
                Name = source.Name
            };
        }
    }
}