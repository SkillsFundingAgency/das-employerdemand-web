using System;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class ProviderCourseDemandDetails
    {
        public Guid DemandId { get; set; }
        public int NumberOfApprentices { get; set; }
        public Location Location { get; set; }

        public static implicit operator ProviderCourseDemandDetails(ProviderEmployerDemandDetailsItem source)
        {
            return new ProviderCourseDemandDetails
            {
                DemandId = source.DemandId,
                NumberOfApprentices = source.Apprentices,
                Location = source.Location
            };
        }
    }
}