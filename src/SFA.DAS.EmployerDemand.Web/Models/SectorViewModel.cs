using System;
using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class SectorViewModel
    {
        public SectorViewModel()
        {

        }

        public SectorViewModel(Sector sector, ICollection<string> selectedSectors)
        {
            Selected = selectedSectors?.Contains(sector.Route) ?? false;
            Id = sector.Id;
            Route = sector.Route;

        }
        public bool Selected { get; }
        public string Route { get; }
        public Guid Id { get; }
    }
}