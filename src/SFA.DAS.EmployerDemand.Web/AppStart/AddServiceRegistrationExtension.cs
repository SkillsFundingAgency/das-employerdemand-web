using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Infrastructure.Api;

namespace SFA.DAS.EmployerDemand.Web.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient<IApiClient, ApiClient>();
            services.AddTransient<IDemandService, DemandService>();
        }
    }
}