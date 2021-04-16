using System.Text.RegularExpressions;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public abstract class CourseDemandViewModelBase
    {
        private const string PostcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s*\d[A-Za-z]{2}$";
        public string Location { get; set; }
        public string LocationName => GetLocationName();

        private string GetLocationName()
        {
            if (string.IsNullOrEmpty(Location))
            {
                return string.Empty;
            }
            
            var locationName = Location;
            
            var locationPostcode = Location.Split(",")[0];
            if (Regex.IsMatch(locationPostcode, PostcodeRegex))
            {
                locationName = locationPostcode;
            }

            return locationName;
        }
    }
}