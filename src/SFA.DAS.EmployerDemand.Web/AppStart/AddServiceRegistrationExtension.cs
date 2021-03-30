using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Application.Locations.Services;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Infrastructure.Api;
using SFA.DAS.EmployerDemand.Infrastructure.Services;

namespace SFA.DAS.EmployerDemand.Web.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            
            services.AddHttpClient<IApiClient, ApiClient>();
            services.AddTransient<IDemandService, DemandService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
        }
    }
}