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
        public bool ShowFilterOptions => ShouldShowFilterOptions();
        public string SelectedCourse { get ; set ; }
        

        public static implicit operator AggregatedProviderCourseDemandViewModel(GetProviderEmployerDemandQueryResult source)
        {
            return new AggregatedProviderCourseDemandViewModel
            {
                SelectedCourse = source.SelectedCourseId != null ? source.Courses.SingleOrDefault(c => c.Id.Equals(source.SelectedCourseId))?.Title : "",
                TotalFiltered = source.TotalFiltered,
                TotalResults = source.TotalResults,
                Courses = source.Courses.Select(c=>(TrainingCourseViewModel)c),
                CourseDemands = source.CourseDemands.Select(c=>(ProviderCourseDemandViewModel)c)
            };
        }
        
        private bool ShouldShowFilterOptions()
        {
            return !string.IsNullOrEmpty(SelectedCourse);
        }
    }
}