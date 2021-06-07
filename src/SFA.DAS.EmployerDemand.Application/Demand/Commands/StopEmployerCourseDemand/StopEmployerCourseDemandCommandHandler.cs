using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerCourseDemand
{
    public class StopEmployerCourseDemandCommandHandler : IRequestHandler<StopEmployerCourseDemandCommand, StopEmployerCourseDemandResult> 
    {
        private readonly IDemandService _demandService;

        public StopEmployerCourseDemandCommandHandler(IDemandService demandService)
        {
            _demandService = demandService;
        }

        public async Task<StopEmployerCourseDemandResult> Handle(StopEmployerCourseDemandCommand request, CancellationToken cancellationToken)
        {
            var result = await _demandService.StopEmployerCourseDemand(request.EmployerDemandId);

            return new StopEmployerCourseDemandResult
            {
                EmployerEmail = result.ContactEmail
            };
        }
    }
}