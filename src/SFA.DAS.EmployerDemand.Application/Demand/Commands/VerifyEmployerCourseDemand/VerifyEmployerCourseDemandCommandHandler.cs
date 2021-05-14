using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerCourseDemand
{
    public class VerifyEmployerCourseDemandCommandHandler : IRequestHandler<VerifyEmployerCourseDemandCommand, VerifyEmployerCourseDemandCommandResult>
    {
        private readonly IDemandService _service;

        public VerifyEmployerCourseDemandCommandHandler (IDemandService service)
        {
            _service = service;
        }
        public async Task<VerifyEmployerCourseDemandCommandResult> Handle(VerifyEmployerCourseDemandCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.VerifyEmployerCourseDemand(request.Id);

            return new VerifyEmployerCourseDemandCommandResult
            {
                EmployerDemand = result
            };
        }
    }
}