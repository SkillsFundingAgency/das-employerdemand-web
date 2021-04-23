using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class SectorViewModel
    {
        public SectorViewModel(Sector sector, IEnumerable<string> selectedSectors)
        {
            Selected = selectedSectors?.Contains(sector.Route) ?? false;
            Id = sector.Id;
            Route = sector.Route;

        }
        public bool Selected { get; }
        public string Route { get; }
        public int Id { get; }
    }
}