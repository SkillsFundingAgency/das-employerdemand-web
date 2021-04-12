using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Locations;

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
        public string Location { get ; set ; }
        public int Ukprn { get; set; }

        public string ClearCourseLink => BuildClearCourseLink();
        public string ClearLocationLink => BuildClearLocationLink();
        public string SelectedRadius { get ; set ; }
        public Dictionary<string, string> LocationRadius => BuildLocationRadiusList();
        private int? SelectedCourseId { get; set; }
        private Location SelectedLocation { get; set; }
        public static implicit operator AggregatedProviderCourseDemandViewModel(GetProviderEmployerDemandQueryResult source)
        {
            var locationList = BuildLocationRadiusList();
            var trainingCourseViewModels = source.Courses.Select(c=>(TrainingCourseViewModel)c).ToList();
            return new AggregatedProviderCourseDemandViewModel
            {
                SelectedCourseId = source.SelectedCourseId,
                SelectedCourse = source.SelectedCourseId != null ? trainingCourseViewModels.SingleOrDefault(c => c.Id.Equals(source.SelectedCourseId))?.TitleAndLevel : "",
                TotalFiltered = source.TotalFiltered,
                TotalResults = source.TotalResults,
                Courses = trainingCourseViewModels,
                CourseDemands = source.CourseDemands.Select(c=>(ProviderCourseDemandViewModel)c),
                SelectedLocation = source.SelectedLocation,
                Location = source.SelectedLocation?.Name,
                SelectedRadius = source.SelectedRadius != null && locationList.ContainsKey(source.SelectedRadius) ? source.SelectedRadius : locationList.First().Key,
                Ukprn = source.Ukprn
            };
        }
        
        private bool ShouldShowFilterOptions()
        {
            return !string.IsNullOrEmpty(SelectedCourse) || !string.IsNullOrEmpty(Location);
        }

        private string BuildClearCourseLink()
        {
            if (SelectedLocation == null)
            {
                return "";    
            }

            return $"?location={HttpUtility.UrlEncode(SelectedLocation.Name)}&radius={SelectedRadius}";
        }

        private string BuildClearLocationLink()
        {
            if (SelectedCourseId == null)
            {
                return "";    
            }
            return $"?selectedCourseId={SelectedCourseId}";
        }
        
        private static Dictionary<string, string> BuildLocationRadiusList()
        {
            return new Dictionary<string, string>
            {
                {"5", "5 miles"},
                {"10", "10 miles"},
                {"25", "25 miles"},
                {"50", "50 miles"},
                {"80", "80 miles"},
                {"1000", "England"},
            };
        }
    }
}