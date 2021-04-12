using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class AggregatedProviderCourseDemandDetailsViewModel
    {
        private const string AllEnglandKey = "1000";
        public TrainingCourseViewModel Course { get ; set ; }
        public IReadOnlyList<ProviderCourseDemandDetailsViewModel> CourseDemandDetailsList { get ; set ; }
        public bool ShowFilterOptions => ShouldShowFilterOptions();
        public string Location { get ; set ; }

        public string ClearLocationLink => "";
        public string SelectedRadius { get ; set ; }
        public Dictionary<string, string> LocationRadius => BuildLocationRadiusList();
        public string CountDescription => BuildCountDescription();
        public IReadOnlyList<Guid> SelectedEmployerDemandIds { get; set; }

        public static implicit operator AggregatedProviderCourseDemandDetailsViewModel(GetProviderEmployerDemandDetailsQueryResult source)
        {
            var locationList = BuildLocationRadiusList();
            return new AggregatedProviderCourseDemandDetailsViewModel
            {
                Course = source.Course,
                CourseDemandDetailsList = source.CourseDemandDetailsList.Select(c=>(ProviderCourseDemandDetailsViewModel)c).ToList(),
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
                {AllEnglandKey, "England"},
            };
        }

        private string BuildCountDescription()
        {
            var countDescription = new StringBuilder();

            countDescription.Append($"{CourseDemandDetailsList.Count} employer");
            if (CourseDemandDetailsList.Count != 1)
            {
                countDescription.Append("s");
            }
            countDescription.Append(" within ");
            if (SelectedRadius == AllEnglandKey)
            {
                countDescription.Append("England.");
            }
            else
            {
                countDescription.Append($"{SelectedRadius} miles of {Location}.");
            }

            return countDescription.ToString();
        }
    }
}