using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCourseDemand
{
    public class CreateCourseDemandCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string ResponseUrl { get ; set ; }
        public string StopSharingUrl { get ; set ; }
    }
}