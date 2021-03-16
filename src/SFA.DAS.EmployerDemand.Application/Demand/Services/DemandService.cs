using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api;
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
        public async Task<TrainingCourse> GetCreateCourseDemand(int trainingCourseId)
        {
            var result =
                await _apiClient.Get<GetCreateCourseDemandResponse>(new GetCreateDemandRequest(trainingCourseId));

            return result?.Course;
        }

        public async Task CreateCacheCourseDemand(ICourseDemand item)
        {
            await _cacheStorageService.SaveToCache(item.Id.ToString(), item, TimeSpan.FromMinutes(30));
        }
    }
}