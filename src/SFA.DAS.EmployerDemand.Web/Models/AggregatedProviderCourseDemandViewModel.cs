using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class AggregatedProviderCourseDemandViewModel : ProviderCourseDemandBaseViewModel
    {
        public IEnumerable<TrainingCourseViewModel> Courses { get ; private set ; }
        public int TotalResults { get ; private set ; }
        public int TotalFiltered { get ; private set ; }
        public IEnumerable<ProviderCourseDemandViewModel> CourseDemands { get ; private set ; }
        public override bool ShowFilterOptions => ShouldShowFilterOptions();
        public string SelectedCourse { get ; private set ; }
        public string Location { get ; private set ; }
        public int Ukprn { get; set; }
        public List<RouteViewModel> Routes { get; private set; }
        public string ClearCourseLink => BuildClearCourseLink();
        public string ClearLocationLink => BuildClearLocationLink();
        public Dictionary<string,string> ClearRouteLinks => BuildClearRouteLinks();
        public string SelectedRadius { get ; private set ; }
        private int? SelectedCourseId { get; set; }
        private Location SelectedLocation { get; set; }
        public List<string> SelectedRoutes { get; private set; }
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
                Ukprn = source.Ukprn,
                Routes = source.Routes.Select(c=>new RouteViewModel(c, source.SelectedRoutes)).ToList(),
                SelectedRoutes = source.SelectedRoutes != null ? source.SelectedRoutes.ToList() : new List<string>()
            };
        }
        
        private bool ShouldShowFilterOptions()
        {
            return !string.IsNullOrEmpty(SelectedCourse) || !string.IsNullOrEmpty(Location) || SelectedRoutes.Any();
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
                if (!SelectedRoutes.Any())
                {
                    return "";
                }

                return "?routes=" + string.Join("&routes=", SelectedRoutes.Select(HttpUtility.UrlEncode));
            }
            return $"?selectedCourseId={SelectedCourseId}";
        }

        private Dictionary<string, string> BuildClearRouteLinks()
        {
            var clearFilterLinks = new Dictionary<string, string>();
            if (SelectedRoutes?.FirstOrDefault() == null)
            {
                return clearFilterLinks;
            }

            var clearFilterString = "";
            
            if (SelectedLocation != null)
            {
                clearFilterString +=
                    $"?location={HttpUtility.UrlEncode(SelectedLocation.Name)}&radius={SelectedRadius}";
            }
            var separator = "&";
            if (string.IsNullOrEmpty(clearFilterString))
            {
                separator = "?";
            }
            

            foreach (var selectedRoute in SelectedRoutes)
            {
                var selectedRouteLink = $"{separator}routes=" + string.Join("&routes=",
                    SelectedRoutes
                        .Where(c => !c.Equals(selectedRoute,
                            StringComparison.CurrentCultureIgnoreCase))
                        .Select(HttpUtility.UrlEncode));

                if (Routes.SingleOrDefault(c=>c.Route.Equals(selectedRoute, StringComparison.CurrentCultureIgnoreCase))!=null)
                {
                    clearFilterLinks.Add(selectedRoute, clearFilterString + selectedRouteLink);
                }
            }
            return clearFilterLinks;
        }
    }
}