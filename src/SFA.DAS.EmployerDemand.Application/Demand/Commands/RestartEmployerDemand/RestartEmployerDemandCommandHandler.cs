using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.RestartEmployerDemand
{
    public class RestartEmployerDemandCommandHandler : IRequestHandler<RestartEmployerDemandCommand, RestartEmployerDemandCommandResult>
    {
        private readonly IDemandService _demandService;

        public RestartEmployerDemandCommandHandler (IDemandService demandService)
        {
            _demandService = demandService;
        }
        public async Task<RestartEmployerDemandCommandResult> Handle(RestartEmployerDemandCommand request, CancellationToken cancellationToken)
        {
            var result = await _demandService.GetRestartCourseDemand(request.EmployerDemandId);

            return new RestartEmployerDemandCommandResult
            {
                Id = result.Id,
                EmailVerified = result.EmailVerified,
                RestartDemandExists = result.RestartDemandExists
            };
        }
    }
}