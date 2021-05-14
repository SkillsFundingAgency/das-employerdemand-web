using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateProviderInterest
{
    public class CreateProviderInterestCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}