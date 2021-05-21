using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenCreatingProviderInterest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Cache_Is_Read_And_Api_Is_Called(
            Guid id,
            ProviderInterestRequest interest,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            PostCreateProviderInterestsRequest actualApiRequest = null;
            cacheStorageService
                .Setup(x => x.RetrieveFromCache<ProviderInterestRequest>(id.ToString()))
                .ReturnsAsync(interest);

            await service.CreateProviderInterest(id);

            apiClient.Verify(x => x.Post<int, PostCreateProviderInterestsData>(
                It.Is<PostCreateProviderInterestsRequest>(request => request.Data.Ukprn == interest.Ukprn)));
        }
    }
}