using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Locations
{
    public class Location
    {
        public string Name { get ; set ; }
        public List<double> LocationPoint { get ; set ; }

        public static implicit operator Location(LocationItem source)
        {
            if (source == null)
            {
                return null;
            }
            
            return new Location
            {
                Name = source.Name,
                LocationPoint = source.LocationPoint.GeoPoint
            };
        }
    }
}