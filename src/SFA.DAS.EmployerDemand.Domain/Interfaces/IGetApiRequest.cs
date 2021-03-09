using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IGetApiRequest
    {
        [JsonIgnore]
        string GetUrl { get; }
    }
}