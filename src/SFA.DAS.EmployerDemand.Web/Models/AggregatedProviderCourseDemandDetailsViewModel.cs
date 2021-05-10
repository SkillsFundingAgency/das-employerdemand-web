using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class AggregatedProviderCourseDemandDetailsViewModel : ProviderCourseDemandBaseViewModel
    {
        public TrainingCourseViewModel Course { get ; set ; }
        public IReadOnlyList<ProviderCourseDemandDetailsViewModel> CourseDemandDetailsList { get ; set ; }
        public override bool ShowFilterOptions => ShouldShowFilterOptions();
        public string Location { get ; set ; }
        public string SelectedRadius { get ; set ; }
        public string CountDescription => BuildCountDescription();
        public IReadOnlyList<Guid> SelectedEmployerDemandIds { get; set; }
        public string ProviderEmail { get ; set ; }
        public string ProviderTelephoneNumber { get ; set ; }
        public string ProviderWebsite { get ; set ; }

        public static implicit operator AggregatedProviderCourseDemandDetailsViewModel(GetProviderEmployerDemandDetailsQueryResult source)
        {
            var locationList = BuildLocationRadiusList();
            return new AggregatedProviderCourseDemandDetailsViewModel
            {
                Course = source.Course,
                CourseDemandDetailsList = source.CourseDemandDetailsList.Select(c=>(ProviderCourseDemandDetailsViewModel)c).ToList(),
                Location = source.SelectedLocation?.Name,
                SelectedRadius = source.SelectedRadius != null && locationList.ContainsKey(source.SelectedRadius) ? source.SelectedRadius : locationList.First().Key,
                ProviderEmail = source.ProviderContactDetails?.EmailAddress ?? string.Empty,
                ProviderWebsite = source.ProviderContactDetails?.Website ?? string.Empty,
                ProviderTelephoneNumber = source.ProviderContactDetails?.PhoneNumber ?? string.Empty,
                SelectedEmployerDemandIds = source.EmployerDemandIds.Any() ? source.EmployerDemandIds.ToList() : new List<Guid>()
            };
        }
        
        private bool ShouldShowFilterOptions()
        {
            return !string.IsNullOrEmpty(Location);
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