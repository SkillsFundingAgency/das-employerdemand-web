using System;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class GetEmployerDemandRequest : IGetApiRequest
    {
        private readonly Guid _id;

        public GetEmployerDemandRequest(Guid id)
        {
            _id = id;
        }

        public string GetUrl => $"demand/{_id}";
    }
}