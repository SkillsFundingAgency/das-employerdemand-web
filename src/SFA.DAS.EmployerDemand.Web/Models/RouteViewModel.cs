using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class RouteViewModel
    {
        public RouteViewModel(string route, IEnumerable<string> selectedRoutes)
        {
            Selected = selectedRoutes?.Contains(route) ?? false;
            Route = route;
            Id = Guid.NewGuid();
        }
        public bool Selected { get; }
        public string Route { get; }
        public Guid Id { get ; }
    }
}