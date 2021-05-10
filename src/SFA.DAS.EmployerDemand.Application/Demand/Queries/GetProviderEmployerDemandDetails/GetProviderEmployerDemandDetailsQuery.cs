using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails
{
    public class GetProviderEmployerDemandDetailsQuery : IRequest<GetProviderEmployerDemandDetailsQueryResult>
    {
        public int Ukprn { get; set; }
        public int CourseId { get; set; }
        public string Location { get ; set ; }
        public string LocationRadius { get ; set ; }
        public string CachedObjectId { get; set; }
    }
}