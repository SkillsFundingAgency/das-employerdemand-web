using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCreateCourseDemand
{
    public class GetCreateCourseDemandQuery : IRequest<GetCreateCourseDemandQueryResult>
    {
        public int TrainingCourseId { get ; set ; }
        public Guid? CreateDemandId { get ; set ; }
    }
}