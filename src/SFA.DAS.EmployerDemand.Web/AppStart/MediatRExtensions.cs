using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Web.AppStart
{
    public static class MediatRExtensions
    {
        public static void AddMediatRValidation(this IServiceCollection services)
        {
            services.AddScoped(typeof(IValidator<CreateCourseDemandCommand>), typeof(CreateCourseDemandCommandValidator));
        }
    }
}