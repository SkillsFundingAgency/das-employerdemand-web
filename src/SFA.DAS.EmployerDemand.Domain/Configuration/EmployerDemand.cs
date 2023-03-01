namespace SFA.DAS.EmployerDemand.Domain.Configuration
{
    public class EmployerDemand
    {
        public string RedisConnectionString { get ; set ; }
        public string FindApprenticeshipTrainingUrl { get; set; }
        public string DataProtectionKeysDatabase { get ; set ; }
        public string ZendeskSectionId { get; set; }
        public string ZendeskSnippetKey { get; set; }
        public string ZendeskCoBrowsingSnippetKey { get; set; }
        public bool UseDfESignIn {get; set; }
    }
}