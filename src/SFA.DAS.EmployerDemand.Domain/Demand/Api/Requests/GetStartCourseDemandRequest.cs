using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class GetStartCourseDemandRequest : IGetApiRequest
    {
        private readonly int _courseId;

        public GetStartCourseDemandRequest(int courseId)
        {
            _courseId = courseId;
        }

        public string GetUrl => $"demand/start/{_courseId}";
    }
}