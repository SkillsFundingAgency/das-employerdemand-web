using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Validation;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest
{
    public class CreateProviderInterestCommandValidator : IValidator<CreateCachedProviderInterestCommand>
    {
        public async Task<ValidationResult> ValidateAsync(CreateCachedProviderInterestCommand item)
        {
            var result = new ValidationResult();

            if (item.EmployerDemandIds == null || !item.EmployerDemandIds.Any())
            {
                result.AddError(nameof(item.EmployerDemandIds), "Select the employers you're interested in");
            }

            return result;
        }
    }
}