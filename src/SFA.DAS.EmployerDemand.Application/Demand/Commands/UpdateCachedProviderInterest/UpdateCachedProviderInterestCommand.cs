using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.UpdateCachedProviderInterest
{
    public class UpdateCachedProviderInterestCommand : IRequest<UpdateCachedProviderInterestCommandResult>
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
    }
}