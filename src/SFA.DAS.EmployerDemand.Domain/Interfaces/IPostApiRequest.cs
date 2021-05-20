using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IPostApiRequest
    {
        [JsonIgnore]
        string PostUrl { get; }
        
    }
    
    public interface IPostApiRequest<TData> : IPostApiRequest
    {
        TData Data { get; set; }
    }
}