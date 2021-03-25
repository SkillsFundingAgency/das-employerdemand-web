using System;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class PostCreateDemandRequest : IPostApiRequest<PostCreateDemandData>
    {
        public PostCreateDemandData Data { get; set; }

        public PostCreateDemandRequest (PostCreateDemandData data)
        {
            Data = data;
        }

        public string PostUrl => "demand/create";
    }

    public class PostCreateDemandResponse
    {
        public Guid Id { get ; set ; }
    }
}