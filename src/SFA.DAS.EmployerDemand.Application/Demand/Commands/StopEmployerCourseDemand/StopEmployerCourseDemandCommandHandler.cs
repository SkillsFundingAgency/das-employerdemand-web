using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerCourseDemand
{
    public class StopEmployerCourseDemandCommandHandler : IRequestHandler<StopEmployerCourseDemandCommand, StopEmployerCourseDemandResult> {
        public async Task<StopEmployerCourseDemandResult> Handle(StopEmployerCourseDemandCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}