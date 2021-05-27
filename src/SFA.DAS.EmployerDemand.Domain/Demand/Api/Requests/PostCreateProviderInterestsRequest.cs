using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class PostCreateProviderInterestsRequest : IPostApiRequest<PostCreateProviderInterestsData>
    {
        public string PostUrl => "providerinterest";
        public PostCreateProviderInterestsData Data { get; set; }

        public PostCreateProviderInterestsRequest(PostCreateProviderInterestsData data)
        {
            Data = data;
        }
    }
}