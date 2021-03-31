using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand
{
    public class GetProviderEmployerDemandQuery : IRequest<GetProviderEmployerDemandQueryResult>
    {
        public int Ukprn { get; set; }
        public int? CourseId { get; set; }
        public string Location { get ; set ; }
    }
}