using System;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand
{
    public class CreateCachedCourseDemandCommand : IRequest<CreateCachedCourseDemandCommandResult>, ICourseDemand
    {
        public Guid Id { get ; set ; }
        public int TrainingCourseId { get; set; }
        public string OrganisationName { get; set; }
        public string NumberOfApprentices { get; set; }
        public string Location { get; set; }
        public string ContactEmailAddress { get ; set ; }
        public Location LocationItem { get; set; }
        public Course Course { get; set; }
        public bool? NumberOfApprenticesKnown { get ; set ; }
        public Guid? ExpiredCourseDemandId { get; set; }
    }
}