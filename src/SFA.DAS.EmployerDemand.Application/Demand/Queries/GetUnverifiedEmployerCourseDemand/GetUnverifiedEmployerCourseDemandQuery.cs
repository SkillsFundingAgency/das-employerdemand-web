using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnverifiedEmployerCourseDemand
{
    public class GetUnverifiedEmployerCourseDemandQuery : IRequest<GetUnverifiedEmployerCourseDemandQueryResult>
    {
        public Guid Id { get ; set ; }
    }
}