using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class AggregatedProviderCourseDemandDetailsViewModel
    {
        public TrainingCourseViewModel Course { get ; set ; }
        public IEnumerable<ProviderCourseDemandDetailsViewModel> CourseDemands { get ; set ; }
        public bool ShowFilterOptions => ShouldShowFilterOptions();
        public string Location { get ; set ; }

        public string ClearLocationLink => "";
        public string SelectedRadius { get ; set ; }
        public Dictionary<string, string> LocationRadius => BuildLocationRadiusList();
        public static implicit operator AggregatedProviderCourseDemandDetailsViewModel(GetProviderEmployerDemandDetailsQueryResult source)
        {
            var locationList = BuildLocationRadiusList();
            return new AggregatedProviderCourseDemandDetailsViewModel
            {
                Course = source.Course,
                CourseDemands = source.CourseDemandDetailsList.Select(c=>(ProviderCourseDemandDetailsViewModel)c),
                Location = source.SelectedLocation?.Name,
                SelectedRadius = source.SelectedRadius != null && locationList.ContainsKey(source.SelectedRadius) ? source.SelectedRadius : locationList.First().Key
            };
        }
        
        private bool ShouldShowFilterOptions()
        {
            return !string.IsNullOrEmpty(Location);
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