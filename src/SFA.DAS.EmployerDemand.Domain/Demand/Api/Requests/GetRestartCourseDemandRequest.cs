using System;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class GetRestartCourseDemandRequest : IGetApiRequest
    {
        private readonly Guid _id;

        public GetRestartCourseDemandRequest(Guid id)
        {
            _id = id;
        }

        public string GetUrl => $"demand/{_id}/restart";
    }
}