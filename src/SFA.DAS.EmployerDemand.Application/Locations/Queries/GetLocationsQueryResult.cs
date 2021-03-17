using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Application.Locations.Queries
{
    public class GetLocationsQueryResult
    {
        public IEnumerable<Domain.Locations.LocationItem> LocationItems { get ; set ; }
    }
}