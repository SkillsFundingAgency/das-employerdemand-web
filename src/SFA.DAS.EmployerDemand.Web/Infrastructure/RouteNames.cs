namespace SFA.DAS.EmployerDemand.Web.Infrastructure
{
    public static class RouteNames
    {
        public const string ServiceStartDefault = "default";
        public const string ServiceStart = "service-start";
        
        public const string RegisterDemand = "register-demand";
        public const string PostRegisterDemand = "register-demand";
        public const string ConfirmRegisterDemand = "confirm-register-demand";
        public const string PostConfirmRegisterDemand = "confirm-register-demand";
        public const string RegisterDemandCompleted = "register-demand-completed";
        
        public const string ProviderDashboard = "provider-demand-dashboard";
        public const string ProviderDemandDetails = "provider-demand-details";
        public const string PostProviderDemandDetails = "post-provider-demand-details";
        public const string ProviderSignOut = "provider-signout";

        public const string ConfirmProviderDetails = "confirm-provider-details";
        public const string EditProviderDetails = "edit-provider-details";
        public const string PostEditProviderDetails = "post-edit-provider-details";
        public const string ReviewProviderDetails = "review-provider-details";
    }
}