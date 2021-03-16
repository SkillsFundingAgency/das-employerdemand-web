using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Locations.Queries
{
    public class GetLocationsQuery : IRequest<GetLocationsQueryResult>
    {
        public string SearchTerm { get ; set ; }
    }
}