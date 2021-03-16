using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries
{
    public class GetCreateCourseDemandQuery : IRequest<GetCreateCourseDemandQueryResult>
    {
        public int TrainingCourseId { get ; set ; }
    }
}