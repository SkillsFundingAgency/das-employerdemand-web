using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Infrastructure.Authorization;

namespace SFA.DAS.EmployerDemand.Web.AppStart
{
    public static class AuthorizationServicePolicyExtension
    {
        
        private const string ProviderDaa = "DAA";

        public static void AddAuthorizationServicePolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    PolicyNames
                        .HasProviderAccount
                    , policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(ProviderClaims.ProviderUkprn);
                        policy.RequireClaim(ProviderClaims.Service, ProviderDaa);
                        policy.Requirements.Add(new ProviderUkPrnRequirement());
                    });
            });
        }
    }
    
}