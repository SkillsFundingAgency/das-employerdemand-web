using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class EmployerDemands
    {
        public Guid EmployerDemandId { get; set; }
        public int NumberOfApprentices { get; set; }
        public string LocationName { get; set; }

        public static implicit operator EmployerDemands(Domain.Demand.EmployerDemands source)
        {
            return new EmployerDemands
            {
                EmployerDemandId = source.EmployerDemandId,
                LocationName = source.LocationName,
                NumberOfApprentices = source.NumberOfApprentices
            };
        }
    }
}