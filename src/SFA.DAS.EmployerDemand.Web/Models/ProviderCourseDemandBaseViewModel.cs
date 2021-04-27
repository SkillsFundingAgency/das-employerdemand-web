using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public abstract class ProviderCourseDemandBaseViewModel
    {
        public abstract bool ShowFilterOptions { get; }
        public Dictionary<string, string> LocationRadius => BuildLocationRadiusList();
        protected const string AllEnglandKey = "1000";
        protected static Dictionary<string, string> BuildLocationRadiusList()
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
    }
}