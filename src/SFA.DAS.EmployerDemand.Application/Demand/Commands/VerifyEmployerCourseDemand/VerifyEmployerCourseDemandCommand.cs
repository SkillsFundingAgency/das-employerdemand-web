using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerCourseDemand
{
    public class VerifyEmployerCourseDemandCommand : IRequest<VerifyEmployerCourseDemandCommandResult>
    {
        public Guid Id { get; set; }
    }
}