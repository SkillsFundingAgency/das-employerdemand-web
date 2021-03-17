using System.Web;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class GetCreateDemandRequest : IGetApiRequest
    {
        private readonly int _trainingCourseId;
        private readonly string _location;

        public GetCreateDemandRequest (int trainingCourseId, string location)
        {
            _trainingCourseId = trainingCourseId;
            _location = location;
        }
        public string GetUrl => $"demand/create/{_trainingCourseId}?location={HttpUtility.UrlEncode(_location)}";
    }
}