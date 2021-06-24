using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand
{
    public class GetStartCourseDemandQueryResult
    {
        public Course Course { get; set; }
        public short? EntryPoint { get ; set ; }
    }
}