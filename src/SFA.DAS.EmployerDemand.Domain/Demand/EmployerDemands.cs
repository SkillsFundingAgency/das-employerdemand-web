using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class EmployerDemands
    {
        public Guid EmployerDemandId { get; set; }
        public int NumberOfApprentices { get; set; }
        public string LocationName { get; set; }
    }
}