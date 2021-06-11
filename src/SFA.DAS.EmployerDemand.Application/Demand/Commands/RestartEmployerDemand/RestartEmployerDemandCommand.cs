using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.RestartEmployerDemand
{
    public class RestartEmployerDemandCommand : IRequest<RestartEmployerDemandCommandResult>
    {
        public Guid EmployerDemandId { get ; set ; }
    }
}