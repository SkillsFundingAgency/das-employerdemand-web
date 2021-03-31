using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class AggregatedProviderCourseDemandViewModel
    {
        public IEnumerable<TrainingCourseViewModel> Courses { get ; set ; }
        public int TotalResults { get ; set ; }
        public int TotalFiltered { get ; set ; }
        public IEnumerable<ProviderCourseDemandViewModel> CourseDemands { get ; set ; }
        public bool ShowFilterOptions { get ; set ; }
        public string SelectedCourse { get ; set ; }
        public string SelectedCourseId { get; set; }

        public static implicit operator AggregatedProviderCourseDemandViewModel(GetProviderEmployerDemandQueryResult source)
        {
            return new AggregatedProviderCourseDemandViewModel
            {
                TotalFiltered = source.TotalFiltered,
                TotalResults = source.TotalResults,
                Courses = source.Courses.Select(c=>(TrainingCourseViewModel)c),
                CourseDemands = source.CourseDemands.Select(c=>(ProviderCourseDemandViewModel)c)
            };
        }
    }
}