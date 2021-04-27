using System.Web;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class GetProviderEmployerDemandDetailsRequest : IGetApiRequest
    {
        private readonly int _ukprn;
        private readonly int _courseId;
        private readonly string _location;
        private readonly string _locationRadius;

        public GetProviderEmployerDemandDetailsRequest(int ukprn, int courseId, string location = "", string locationRadius = "")
        {
            _ukprn = ukprn;
            _courseId = courseId;
            _location = location;
            _locationRadius = locationRadius;
        }

        public string GetUrl => $"demand/providers/{_ukprn}/courses/{_courseId}?location={HttpUtility.UrlEncode(_location)}&locationRadius={_locationRadius}";
    }
}
