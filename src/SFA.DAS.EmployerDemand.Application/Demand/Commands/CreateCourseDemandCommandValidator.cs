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

            if (string.IsNullOrEmpty(item.Location))
            {
                validationResult.AddError(nameof(item.Location), "Enter a town, city or postcode");
            }

            if (!item.NumberOfApprenticesKnown.HasValue)
            {
                validationResult.AddError(nameof(item.NumberOfApprenticesKnown),"Select yes if you know how many apprentices will take this apprenticeship training");
            }
            if (item.NumberOfApprenticesKnown != null && item.NumberOfApprenticesKnown.Value)
            {
                if (!item.NumberOfApprentices.HasValue || item.NumberOfApprentices.Value == 0)
                {
                    validationResult.AddError(nameof(item.NumberOfApprentices),"Enter the number of apprentices");    
                }
                if (item.NumberOfApprentices.HasValue && item.NumberOfApprentices.Value < 0)
                {
                    validationResult.AddError(nameof(item.NumberOfApprentices),"Number of apprentices must be 1 or more");    
                }
                if (item.NumberOfApprentices.HasValue && item.NumberOfApprentices.Value > 9999)
                {
                    validationResult.AddError(nameof(item.NumberOfApprentices),"Number of apprentices must be 9999 or less");    
                }
            }

            if (string.IsNullOrEmpty(item.OrganisationName))
            {
                validationResult.AddError(nameof(item.OrganisationName), "Enter the name of the organisation");
            }

            if (string.IsNullOrEmpty(item.ContactEmailAddress))
            {
                validationResult.AddError(nameof(item.ContactEmailAddress), "Enter an email address");
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
                catch (FormatException)
                {
                    validationResult.AddError(nameof(item.ContactEmailAddress),"Enter an email address in the correct format, like name@example.com");
                }
            }

            return Task.FromResult(validationResult);
        }
    }
}