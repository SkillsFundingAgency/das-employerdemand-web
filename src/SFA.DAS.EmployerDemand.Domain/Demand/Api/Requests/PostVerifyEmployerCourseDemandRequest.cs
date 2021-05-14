using System;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class PostVerifyEmployerCourseDemandRequest : IPostApiRequest<object>
    {
        private readonly Guid _id;

        public PostVerifyEmployerCourseDemandRequest(Guid id)
        {
            _id = id;
        }

        public string PostUrl => $"demand/{_id}/verify";
        public object Data { get; set; }
    }
}