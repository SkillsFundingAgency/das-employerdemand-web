using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IPostApiRequest<TData>
    {
        [JsonIgnore]
        string PostUrl { get; }
        
        TData Data { get; set; }
    }
}