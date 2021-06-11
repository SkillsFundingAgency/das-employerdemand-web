using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCourseDemand
{
    public class CreateCourseDemandCommandHandler : IRequestHandler<CreateCourseDemandCommand, Unit>
    {
        private readonly IDemandService _service;

        public CreateCourseDemandCommandHandler (IDemandService service)
        {
            _service = service;
        }
        public async Task<Unit> Handle(CreateCourseDemandCommand request, CancellationToken cancellationToken)
        {
            await _service.CreateCourseDemand(request.Id, request.ResponseUrl, request.StopSharingUrl, request.StartSharingUrl);
            
            return Unit.Value;
        }
    }
}