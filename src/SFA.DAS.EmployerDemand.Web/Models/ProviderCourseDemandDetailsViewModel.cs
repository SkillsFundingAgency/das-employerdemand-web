using System;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ProviderCourseDemandDetailsViewModel
    {
        public Guid DemandId { get ; set ; }
        public int NumberOfApprentices { get ; set ; }
        public LocationViewModel Location { get; set; }

        public static implicit operator ProviderCourseDemandDetailsViewModel(ProviderCourseDemandDetails source)
        {
            return new ProviderCourseDemandDetailsViewModel
            {
                DemandId = source.DemandId,
                NumberOfApprentices = source.NumberOfApprentices,
                Location = source.Location
            };
        }
    }
}