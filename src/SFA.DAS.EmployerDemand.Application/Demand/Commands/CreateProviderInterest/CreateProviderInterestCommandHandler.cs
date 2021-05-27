using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateProviderInterest
{
    public class CreateProviderInterestCommandHandler : IRequestHandler<CreateProviderInterestCommand, Unit>
    {
        private readonly IDemandService _demandService;

        public CreateProviderInterestCommandHandler(IDemandService demandService)
        {
            _demandService = demandService;
        }

        public async Task<Unit> Handle(CreateProviderInterestCommand request, CancellationToken cancellationToken)
        {
            await _demandService.CreateProviderInterest(request.Id);
            return Unit.Value;
        }
    }
}