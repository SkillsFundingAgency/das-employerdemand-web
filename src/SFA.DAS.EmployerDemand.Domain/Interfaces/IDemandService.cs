using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IDemandService
    {
        Task<GetCreateCourseDemandResponse> GetCreateCourseDemand(int trainingCourseId, string locationName);
        Task CreateCacheCourseDemand(ICourseDemand item);
        Task<CourseDemandRequest> GetCachedCourseDemand(Guid itemKey);
        Task CreateCourseDemand(Guid id);
        Task<GetProviderEmployerDemandResponse> GetProviderEmployerDemand(    int ukprn, int? courseId, string location, string locationRadius, List<string> selectedRoutes);
        Task<GetProviderEmployerDemandDetailsResponse> GetProviderEmployerDemandDetails( int ukprn, int courseId, string location, string locationRadius);
        Task CreateCachedProviderInterest(IProviderDemandInterest item);
        Task<IProviderDemandInterest> GetCachedProviderInterest(Guid itemKey);
        Task<CourseDemand> GetUnverifiedEmployerCourseDemand(Guid id);
        Task<VerifiedCourseDemand> VerifyEmployerCourseDemand(Guid id);
    }
}