using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Validation;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.UpdateCachedProviderInterest
{
    public class UpdateCachedProviderInterestCommandValidator : IValidator<UpdateCachedProviderInterestCommand>
    {
        public Task<ValidationResult> ValidateAsync(UpdateCachedProviderInterestCommand item)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(item.PhoneNumber))
            {
                validationResult.AddError(nameof(item.PhoneNumber), "Enter a telephone number");
            } 
            else if (!Regex.IsMatch(item.PhoneNumber, @"^[0-9extEXTOoPpIiNn \:\+\(\)\#\/]*$") || item.PhoneNumber.Length > 50)
            {
                validationResult.AddError(nameof(item.PhoneNumber), "Enter a telephone number, like 01632 960 001, 07700 900 982 or +44 0808 157 0192");
            }
            
            if (string.IsNullOrEmpty(item.EmailAddress))
            {
                validationResult.AddError(nameof(item.EmailAddress), "Enter an email address");
            }
            else
            {
                try
                {
                    var emailAddress = new MailAddress(item.EmailAddress);
                    if (!emailAddress.Address.Equals(item.EmailAddress, StringComparison.CurrentCultureIgnoreCase))
                    {
                        validationResult.AddError(nameof(item.EmailAddress));
                    }
                }
                catch (FormatException)
                {
                    validationResult.AddError(nameof(item.EmailAddress),"Enter an email address in the correct format, like name@example.com");
                }
            }
            
            return Task.FromResult(validationResult);
        }
    }
}