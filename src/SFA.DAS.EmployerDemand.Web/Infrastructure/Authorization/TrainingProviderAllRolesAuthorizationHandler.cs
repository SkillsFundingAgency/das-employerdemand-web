using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.ProviderAccounts.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.ProviderAccounts.Api.Responses;
using SFA.DAS.Provider.Shared.UI.Models;

namespace SFA.DAS.EmployerDemand.Web.Infrastructure.Authorization;

public class TrainingProviderAllRolesAuthorizationHandler : AuthorizationHandler<TrainingProviderAllRolesRequirement>
{
    private readonly IApiClient _apiClient;
    private readonly HttpContextAccessor _httpContextAccessor;
    private readonly ProviderSharedUIConfiguration _providerSharedUiConfiguration;

    public TrainingProviderAllRolesAuthorizationHandler(IApiClient apiClient, ProviderSharedUIConfiguration providerSharedUiConfiguration)
    {
        _apiClient = apiClient;
        _providerSharedUiConfiguration = providerSharedUiConfiguration;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TrainingProviderAllRolesRequirement requirement)
    {
        HttpContext currentContext;
        switch (context.Resource)
        {
            case HttpContext resource:
                currentContext = resource;
                break;
            case AuthorizationFilterContext authorizationFilterContext:
                currentContext = authorizationFilterContext.HttpContext;
                break;
            default:
                currentContext = null;
                break;
        }
        
        if (!context.User.HasClaim(c => c.Type.Equals(ProviderClaims.ProviderUkprn)))
        {
            context.Fail();
            return;
        }

        var claimValue = context.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;

        if (!int.TryParse(claimValue, out var ukprn))
        {
            context.Fail();
            return;
        }
        
        var result = await _apiClient.Get<GetProviderAccountResponse>(new GetProviderAccountRequest(ukprn));
        if (!result.CanAccessService)
        {
            currentContext?.Response.Redirect($"{_providerSharedUiConfiguration.DashboardUrl}/error/403/invalid-status");    
        }
        context.Succeed(requirement);
    }
}