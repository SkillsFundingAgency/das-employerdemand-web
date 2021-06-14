using System;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class PostStopEmployerCourseDemandRequest : IPostApiRequest<object>
    {
        private readonly Guid _employerDemandId;

        public PostStopEmployerCourseDemandRequest(Guid employerDemandId)
        {
            _employerDemandId = employerDemandId;
        }

        public string PostUrl => $"demand/{_employerDemandId}/stop";
        public object Data { get; set; }
    }
}