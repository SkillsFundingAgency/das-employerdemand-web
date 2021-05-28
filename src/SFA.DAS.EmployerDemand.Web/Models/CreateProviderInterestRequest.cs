using System;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateProviderInterest;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class CreateProviderInterestRequest
    {
        [FromRoute]
        public Guid Id { get; set; }
        [FromRoute]
        public int Ukprn { get; set; }
        [FromRoute]
        public int CourseId { get; set; }

        public static implicit operator CreateProviderInterestCommand(CreateProviderInterestRequest source)
        {
            return new CreateProviderInterestCommand
            {
                Id = source.Id
            };
        }
    }
}