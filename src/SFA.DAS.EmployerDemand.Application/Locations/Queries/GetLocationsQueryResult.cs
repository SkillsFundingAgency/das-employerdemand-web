using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Application.Locations.Queries
{
    public class GetLocationsQueryResult
    {
        public IEnumerable<Location> LocationItems { get ; set ; }
    }
}