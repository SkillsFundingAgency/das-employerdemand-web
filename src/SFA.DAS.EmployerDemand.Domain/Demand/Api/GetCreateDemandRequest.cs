using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api
{
    public class GetCreateDemandRequest : IGetApiRequest
    {
        private readonly int _trainingCourseId;

        public GetCreateDemandRequest (int trainingCourseId)
        {
            _trainingCourseId = trainingCourseId;
        }
        public string GetUrl => $"demand/create/{_trainingCourseId}";
    }
}