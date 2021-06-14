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

        public async Task<RestartCourseDemand> GetRestartCourseDemand(Guid id)
        {
            var result = await _apiClient.Get<GetRestartCourseDemandResponse>(new GetRestartCourseDemandRequest(id));

            var demandId = result.Id;

            if (!result.RestartDemandExists || (result.RestartDemandExists && !result.EmailVerified))
            {
                demandId = Guid.NewGuid();
                var item = new CourseDemand
                {
                    Id = demandId,
                    Location = result.Location.Name,
                    Course = result.Course,
                    EmailVerified = false,
                    LocationItem = result.Location,
                    OrganisationName = result.OrganisationName,
                    ContactEmailAddress = result.ContactEmail,
                    NumberOfApprentices = result.NumberOfApprentices.ToString(),
                    TrainingCourseId = result.Course.Id,
                    NumberOfApprenticesKnown = result.NumberOfApprentices > 0,
                    ExpiredCourseDemandId = result.Id
                };
                
                await _cacheStorageService.SaveToCache(demandId.ToString(), item, TimeSpan.FromMinutes(30));
            }
            
            return new RestartCourseDemand
            {
                Id = demandId,
                TrainingCourseId = result.Course.Id,
                EmailVerified = result.EmailVerified,
                RestartDemandExists = result.RestartDemandExists
            };
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

        public async Task CreateCourseDemand(Guid id, string responseUrl, string stopSharingUrl, string startSharingUrl)
        {
            var item = await _cacheStorageService.RetrieveFromCache<CourseDemandRequest>(id.ToString());

            var data = new PostCreateDemandData(item)
            {
                ResponseUrl = responseUrl,
                StopSharingUrl = stopSharingUrl,
                StartSharingUrl = startSharingUrl
            };

            await _apiClient.Post<Guid, PostCreateDemandData>(new PostCreateDemandRequest(data));
        }

        public async Task<GetProviderEmployerDemandResponse> GetProviderEmployerDemand(int ukprn, int? courseId, string location, string locationRadius, List<string> selectedRoutes)
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
            var apiResult = await _apiClient.Get<GetCourseDemandResponse>(new GetEmployerDemandRequest(id));

            if (apiResult == null)
            {
                return null;
            }
            
            var demand = new CourseDemand
            {
                Id = apiResult.Id,
                EmailVerified = apiResult.EmailVerified,
                ContactEmailAddress = apiResult.ContactEmail
            };
            
            return demand;
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

        public async Task<StoppedCourseDemand> StopEmployerCourseDemand(Guid id)
        {
            var result = await _apiClient.Post<StopEmployerCourseDemandResponse, object>(
                new PostStopEmployerCourseDemandRequest(id));

            return (StoppedCourseDemand)result;
        }
    }
}