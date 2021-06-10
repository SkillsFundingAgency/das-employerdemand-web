using System;
using System.Collections.Generic;
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
        private readonly IFatUrlBuilder _fatUrlBuilder;

        public DemandService (
            IApiClient apiClient, 
            ICacheStorageService cacheStorageService,
            IFatUrlBuilder fatUrlBuilder)
        {
            _apiClient = apiClient;
            _cacheStorageService = cacheStorageService;
            _fatUrlBuilder = fatUrlBuilder;
        }

        public async Task<GetStartCourseDemandResponse> GetStartCourseDemand(int trainingCourseId)
        {
            var result =
                await _apiClient.Get<GetStartCourseDemandResponse>(new GetStartCourseDemandRequest(trainingCourseId));
            return result;
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

        public async Task CreateCourseDemand(Guid id, string responseUrl)
        {
            var item = await _cacheStorageService.RetrieveFromCache<CourseDemandRequest>(id.ToString());

            var data = new PostCreateDemandData(item)
            {
                ResponseUrl = responseUrl
            };

            await _apiClient.Post<Guid, PostCreateDemandData>(new PostCreateDemandRequest(data));

        }

        public async Task<GetProviderEmployerDemandResponse> GetProviderEmployerDemand(    int ukprn, int? courseId,
            string location, string locationRadius, List<string> selectedRoutes)
        {
            var result =
                await _apiClient.Get<GetProviderEmployerDemandResponse>(
                    new GetProviderEmployerDemandRequest(ukprn, courseId, location, locationRadius, selectedRoutes));

            return result;
        }

        public async Task<GetProviderEmployerDemandDetailsResponse> GetProviderEmployerDemandDetails( int ukprn, int courseId, string location, string locationRadius)
        {
            var result =
                await _apiClient.Get<GetProviderEmployerDemandDetailsResponse>(
                    new GetProviderEmployerDemandDetailsRequest(ukprn, courseId, location, locationRadius));

            return result;
        }

        public async Task CreateCachedProviderInterest(IProviderDemandInterest item)
        {
            await _cacheStorageService.SaveToCache(item.Id.ToString(), item, TimeSpan.FromMinutes(30));
        }

        public async Task<IProviderDemandInterest> GetCachedProviderInterest(Guid itemKey)
        {
            var result = await _cacheStorageService.RetrieveFromCache<ProviderInterestRequest>(itemKey.ToString());

            return result;
        }

        public async Task<CourseDemand> GetUnverifiedEmployerCourseDemand(Guid id)
        {
            var cachedResultTask = _cacheStorageService.RetrieveFromCache<CourseDemand>(id.ToString());
            var apiResultTask = _apiClient.Get<GetCourseDemandResponse>(new GetEmployerDemandRequest(id));

            await Task.WhenAll(cachedResultTask, apiResultTask);

            if (cachedResultTask.Result == null || apiResultTask.Result == null)
            {
                return null;
            }

            cachedResultTask.Result.EmailVerified =  apiResultTask.Result.EmailVerified;
            
            return cachedResultTask.Result;
        }

        public async Task<VerifiedCourseDemand> VerifyEmployerCourseDemand(Guid id)
        {
            var result =
                await _apiClient.Post<VerifyEmployerCourseDemandResponse,object>(
                    new PostVerifyEmployerCourseDemandRequest(id));

            return result;
        }

        public async Task CreateProviderInterest(Guid id)
        {
            var item = await _cacheStorageService.RetrieveFromCache<ProviderInterestRequest>(id.ToString());

            var data = new PostCreateProviderInterestsData(item, _fatUrlBuilder.BuildFatUrl);
            
            await _apiClient.Post<Guid, PostCreateProviderInterestsData>(new PostCreateProviderInterestsRequest(data));
        }
    }
}