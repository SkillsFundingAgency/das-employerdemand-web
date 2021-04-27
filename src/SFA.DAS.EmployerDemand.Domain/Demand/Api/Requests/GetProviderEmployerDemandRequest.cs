using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class GetProviderEmployerDemandRequest : IGetApiRequest
    {
        private readonly int _ukprn;
        private readonly int? _courseId;
        private readonly string _location;
        private readonly string _locationRadius;
        private readonly List<string> _selectedRoutes;

        public GetProviderEmployerDemandRequest(int ukprn, int? courseId = null, string location = "", string locationRadius = "", List<string> selectedRoutes = null)
        {
            _ukprn = ukprn;
            _courseId = courseId;
            _location = location;
            _locationRadius = locationRadius;
            _selectedRoutes = selectedRoutes == null || courseId != null ? new List<string>() : selectedRoutes;
        }

        public string GetUrl => $"demand/aggregated/providers/{_ukprn}?courseId={_courseId}&location={HttpUtility.UrlEncode(_location)}&locationRadius={_locationRadius}&routes=" 
                                + string.Join("&routes=", _selectedRoutes.Select(HttpUtility.UrlEncode));
    }
}