using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand
{
    public class GetStartCourseDemandQuery : IRequest<GetStartCourseDemandQueryResult>
    {
        public int TrainingCourseId { get; set; }
        public short? EntryPoint { get ; set ; }
    }
}