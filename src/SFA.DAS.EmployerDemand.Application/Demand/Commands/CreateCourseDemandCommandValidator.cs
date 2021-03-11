using System;
using System.Net.Mail;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Validation;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands
{
    public class CreateCourseDemandCommandValidator : IValidator<CreateCourseDemandCommand>
    {
        public Task<ValidationResult> ValidateAsync(CreateCourseDemandCommand item)
        {
            var validationResult = new ValidationResult();

            if (item.TrainingCourseId == 0)
            {
                validationResult.AddError(nameof(item.TrainingCourseId));
            }

            if (string.IsNullOrEmpty(item.OrganisationName))
            {
                validationResult.AddError(nameof(item.OrganisationName));
            }
            
            if (string.IsNullOrEmpty(item.ContactEmailAddress))
            {
                validationResult.AddError(nameof(item.ContactEmailAddress));
            }
            else
            {
                try
                {
                    var emailAddress = new MailAddress(item.ContactEmailAddress);
                    if (!emailAddress.Address.Equals(item.ContactEmailAddress, StringComparison.CurrentCultureIgnoreCase))
                    {
                        validationResult.AddError(nameof(item.ContactEmailAddress));
                    }
                }
                catch (FormatException e)
                {
                    validationResult.AddError(nameof(item.ContactEmailAddress));
                }
                
            }

            return Task.FromResult(validationResult);
        }
    }
}