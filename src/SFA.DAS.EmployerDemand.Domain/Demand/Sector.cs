using System;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class Sector
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("route")]
        public string Route { get; set; }
    }
}