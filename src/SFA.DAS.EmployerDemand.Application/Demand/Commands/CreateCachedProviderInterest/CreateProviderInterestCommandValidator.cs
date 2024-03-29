﻿using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Validation;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest
{
    public class CreateProviderInterestCommandValidator : IValidator<CreateCachedProviderInterestCommand>
    {
        public Task<ValidationResult> ValidateAsync(CreateCachedProviderInterestCommand item)
        {
            var result = new ValidationResult();

            if (!item.EmployerDemands.Any())
            {
                result.AddError(nameof(item.EmployerDemands), "Select the employers you're interested in");
            }

            return Task.FromResult(result);
        }
    }
}