using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class SectorViewModel
    {
        public SectorViewModel(string sector, IEnumerable<string> selectedSectors)
        {
            Selected = selectedSectors?.Contains(sector) ?? false;
            Route = sector;
            Id = Guid.NewGuid();
        }
        public bool Selected { get; }
        public string Route { get; }
        public Guid Id { get ; }
    }
}