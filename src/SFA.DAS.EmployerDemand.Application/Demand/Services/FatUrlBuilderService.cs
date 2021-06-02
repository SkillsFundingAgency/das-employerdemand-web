using Microsoft.Extensions.Options;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Services
{
    public class FatUrlBuilderService : IFatUrlBuilder
    {
        private readonly Domain.Configuration.EmployerDemand _config;

        public FatUrlBuilderService(IOptions<Domain.Configuration.EmployerDemand> options)
        {
            _config = options.Value;
        }

        public string BuildFatUrl(IProviderDemandInterest interest)
        {
            return interest.ProviderOffersThisCourse ? 
                $"{_config.FindApprenticeshipTrainingUrl}/courses/{interest.Course.Id}/providers/{interest.Ukprn}" : 
                null;
        }
    }
}