using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
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
        public List<SectorViewModel> Sectors { get; set; }

        public string ClearCourseLink => BuildClearCourseLink();
        public string ClearLocationLink => BuildClearLocationLink();
        public Dictionary<string,string> ClearSectorLink => BuildClearSectorLink();
        public string SelectedRadius { get ; set ; }
        public Dictionary<string, string> LocationRadius => BuildLocationRadiusList();
        public int? SelectedCourseId { get; set; }
        public Location SelectedLocation { get; set; }
        public List<string> SelectedSectors { get; set; }
        
        private bool ShouldShowFilterOptions()
        {
            return !string.IsNullOrEmpty(SelectedCourse) || !string.IsNullOrEmpty(Location) || SelectedSectors.Any();
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
                if (!SelectedSectors.Any())
                {
                    return "";
                }

                return "?sectors=" + string.Join("&sectors=", SelectedSectors.Select(HttpUtility.HtmlEncode));
            }
            return $"?selectedCourseId={SelectedCourseId}";
        }

        private Dictionary<string, string> BuildClearSectorLink()
        {
            var clearFilterLinks = new Dictionary<string, string>();
            if (SelectedSectors?.FirstOrDefault() == null)
            {
                return clearFilterLinks;
            }

            var clearFilterString = "";

            if (SelectedCourseId != null)
            {
                clearFilterString += $"?selectedCourseId={SelectedCourseId}";
            }

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

            foreach (var selectedSector in SelectedSectors)
            {
                clearFilterString += $"{separator}sectors" + string.Join("&sectors=",
                    SelectedSectors
                        .Where(c => !c.Equals(selectedSector,
                            StringComparison.CurrentCultureIgnoreCase))
                        .Select(HttpUtility.HtmlEncode));

                var sector =
                    Sectors.SingleOrDefault(c =>
                        c.Route.Equals(selectedSector, StringComparison.CurrentCultureIgnoreCase));

                if (sector != null)
                {
                    clearFilterLinks.Add(sector.Route, clearFilterString);
                }
            }

            return clearFilterLinks;
        }

        public static Dictionary<string, string> BuildLocationRadiusList()
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