using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand
{
    public class GetProviderEmployerDemandQueryResult
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<ProviderCourseDemand> CourseDemands { get; set; }
        public int TotalResults { get; set; }
        public int TotalFiltered { get; set; }
        public int? SelectedCourseId { get; set; }
    }
}