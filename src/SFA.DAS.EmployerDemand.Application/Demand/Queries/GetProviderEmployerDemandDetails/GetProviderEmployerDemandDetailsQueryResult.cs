using System;
using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails
{
    public class GetProviderEmployerDemandDetailsQueryResult
    {
        public Course Course { get; set; }
        public IEnumerable<ProviderCourseDemandDetails> CourseDemandDetailsList { get; set; }
        public Location SelectedLocation { get; set; }
        public string SelectedRadius { get ; set ; }
        public ProviderContactDetails ProviderContactDetails { get ; set ; }
        public IEnumerable<EmployerDemands> EmployerDemands { get; set; }
        public Guid? Id { get; set; }
    }
}