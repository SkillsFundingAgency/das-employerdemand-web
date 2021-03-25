using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand
{
    public class GetCachedCreateCourseDemandQuery : IRequest<GetCachedCreateCourseDemandQueryResult>
    {
        public Guid Id { get ; set ; }
    }
}