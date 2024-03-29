using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Validation;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand
{
    public class CreateCourseDemandCommandValidator : IValidator<CreateCachedCourseDemandCommand>
    {
        public Task<ValidationResult> ValidateAsync(CreateCachedCourseDemandCommand item)
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
            else
            {
                if (ContainsSpecialCharacters(item.Location))
                {
                    validationResult.AddError(nameof(item.Location), "Enter a real town, city or postcode");
                }
            }

            if (!item.NumberOfApprenticesKnown.HasValue)
            {
                validationResult.AddError(nameof(item.NumberOfApprenticesKnown),"Select yes if you know how many apprentices will take this apprenticeship training");
            }
            if (item.NumberOfApprenticesKnown != null && item.NumberOfApprenticesKnown.Value)
            {
                validationResult = CheckNumberOfApprentices(item, validationResult);
            }

            if (string.IsNullOrEmpty(item.OrganisationName))
            {
                validationResult.AddError(nameof(item.OrganisationName), "Enter the name of the organisation");
            }
            else
            {
                if (ContainsSpecialCharacters(item.OrganisationName))
                {
                    validationResult.AddError(nameof(item.OrganisationName), "Enter a valid organisation name");
                }
            }

            validationResult = CheckEmail(item, validationResult);

            return Task.FromResult(validationResult);
        }

        private ValidationResult CheckNumberOfApprentices(CreateCachedCourseDemandCommand item, ValidationResult validationResult)
        {
            var tryConvert = int.TryParse(item.NumberOfApprentices, out var numberOfApprentices);

            if (!tryConvert && numberOfApprentices == 0 && string.IsNullOrEmpty(item.NumberOfApprentices))
            {
                validationResult.AddError(nameof(item.NumberOfApprentices), "Enter the number of apprentices");
            }
            if (!tryConvert && numberOfApprentices == 0 && !string.IsNullOrEmpty(item.NumberOfApprentices))
            {
                validationResult.AddError(nameof(item.NumberOfApprentices), "Number of apprentices must be 9999 or less");
            }
            if (tryConvert && numberOfApprentices == 0)
            {
                validationResult.AddError(nameof(item.NumberOfApprentices), "Enter the number of apprentices");
            }
            if (tryConvert && numberOfApprentices < 0)
            {
                validationResult.AddError(nameof(item.NumberOfApprentices), "Number of apprentices must be 1 or more");
            }
            if (tryConvert && numberOfApprentices > 9999)
            {
                validationResult.AddError(nameof(item.NumberOfApprentices), "Number of apprentices must be 9999 or less");
            }
            return validationResult;
        }

        private ValidationResult CheckEmail(CreateCachedCourseDemandCommand item, ValidationResult validationResult)
        {
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
                    validationResult.AddError(nameof(item.ContactEmailAddress), "Enter an email address in the correct format, like name@example.com");
                }
            }

            return validationResult;
        }

        private bool ContainsSpecialCharacters(string input)
        {
            return !Regex.IsMatch(input, @"^[a-zA-Z0-9 ,\-\'\&@£¥$€#.:;]*$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200));
        }
    }
}