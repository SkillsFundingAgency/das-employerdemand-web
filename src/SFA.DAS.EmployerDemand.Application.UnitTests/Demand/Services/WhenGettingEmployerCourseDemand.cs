using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenGettingEmployerCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Retrieved_From_Cache_And_Api(
            Guid employerDemandKey,
            CourseDemand item,
            GetCourseDemandResponse apiResponse,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            item.EmailVerified = false;
            apiResponse.EmailVerified = true;
            cacheStorageService.Setup(x => x.RetrieveFromCache<CourseDemand>(employerDemandKey.ToString()))
                .ReturnsAsync(item);
            apiClient.Setup(x =>
                    x.Get<GetCourseDemandResponse>(It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{employerDemandKey}"))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetUnverifiedEmployerCourseDemand(employerDemandKey);

            //Assert
            actual.Should().BeEquivalentTo(item);
            actual.EmailVerified.Should().Be(apiResponse.EmailVerified);

        }
        
        [Test, MoqAutoData]
        public async Task Then_If_It_Does_Not_Exist_In_The_Cache_Null_Is_Returned(
            Guid employerDemandKey,
            CourseDemandRequest item,
            [Frozen] Mock<IApiClient> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            DemandService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<ICourseDemand>(It.IsAny<string>()))
                .ReturnsAsync((ICourseDemand)null);
            
            //Act
            var actual = await service.GetUnverifiedEmployerCourseDemand(employerDemandKey);

            //Assert
            actual.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Does_Not_Exist_On_The_Api_Null_Returned(
            Guid employerDemandKey,
            CourseDemand item,
            [Frozen] Mock<IApiClient> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            DemandService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<CourseDemand>(employerDemandKey.ToString()))
                .ReturnsAsync(item);
            apiClient.Setup(x =>
                    x.Get<GetCourseDemandResponse>(It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{employerDemandKey}"))))
                .ReturnsAsync((GetCourseDemandResponse)null);
            
            //Act
            var actual = await service.GetUnverifiedEmployerCourseDemand(employerDemandKey);

            //Assert
            actual.Should().BeNull();
        }
    }
}