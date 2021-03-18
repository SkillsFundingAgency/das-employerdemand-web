using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Services
{
    public class DemandService : IDemandService
    {
        private readonly IApiClient _apiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public DemandService (IApiClient apiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _cacheStorageService = cacheStorageService;
        }
        public async Task<GetCreateCourseDemandResponse> GetCreateCourseDemand(int trainingCourseId, string locationName)
        {
            var result =
                await _apiClient.Get<GetCreateCourseDemandResponse>(new GetCreateDemandRequest(trainingCourseId, locationName));

            return result;
        }

        public async Task CreateCacheCourseDemand(ICourseDemand item)
        {
            await _cacheStorageService.SaveToCache(item.Id.ToString(), item, TimeSpan.FromMinutes(30));
        }

        public async Task<CourseDemandRequest> GetCachedCourseDemand(Guid itemKey)
        {
            var result = await _cacheStorageService.RetrieveFromCache<CourseDemandRequest>(itemKey.ToString());

            return result;
        }

        public async Task CreateCourseDemand(Guid id)
        {
            var item = await _cacheStorageService.RetrieveFromCache<PostCreateDemandData>(id.ToString());

            var result = await _apiClient.Post<Guid, PostCreateDemandData>(new PostCreateDemandRequest(item));

            await _cacheStorageService.DeleteFromCache(result.ToString());
        }
    }
}