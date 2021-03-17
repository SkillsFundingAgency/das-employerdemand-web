using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IDemandService
    {
        Task<GetCreateCourseDemandResponse> GetCreateCourseDemand(int trainingCourseId, string locationName);
        Task CreateCacheCourseDemand(ICourseDemand item);
        Task<ICourseDemand> GetCachedCourseDemand(Guid itemKey);
    }
}