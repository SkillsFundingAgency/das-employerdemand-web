using System;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands
{
    public class CreateCourseDemandCommand : IRequest<CreateCachedCourseDemandCommandResult>, ICourseDemand
    {
        public Guid Id { get ; set ; }
        public int TrainingCourseId { get; set; }
        public string OrganisationName { get; set; }
        public uint? NumberOfApprentices { get; set; }
        public string Location { get; set; }
        public string ContactEmailAddress { get ; set ; }
    }
}