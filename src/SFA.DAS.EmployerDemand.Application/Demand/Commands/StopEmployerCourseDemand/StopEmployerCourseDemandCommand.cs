using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerCourseDemand
{
    public class StopEmployerCourseDemandCommand : IRequest<StopEmployerCourseDemandResult>
    {
        public Guid EmployerDemandId { get; set; }
    }
}