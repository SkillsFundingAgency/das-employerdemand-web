using System.Threading.Tasks;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<TResponse> Post<TResponse, TPostData>(IPostApiRequest<TPostData> request);
    }
}