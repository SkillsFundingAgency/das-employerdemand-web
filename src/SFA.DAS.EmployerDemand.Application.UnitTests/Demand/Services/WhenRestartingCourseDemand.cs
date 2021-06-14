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
    public class WhenRestartingCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Data_Returned_And_Not_Added_To_Cache_If_DemandExists(
            Guid id,
            string locationName,
            GetRestartCourseDemandResponse apiResponse,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiResponse.RestartDemandExists = true;
            apiClient.Setup(x =>
                    x.Get<GetRestartCourseDemandResponse>(
                        It.Is<GetRestartCourseDemandRequest>(c => c.GetUrl.Contains(id.ToString()))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetRestartCourseDemand(id);
            
            //Assert
            actual.Id.Should().Be(apiResponse.Id);
            actual.EmailVerified.Should().Be(apiResponse.EmailVerified);
            actual.RestartDemandExists.Should().Be(apiResponse.RestartDemandExists);
            cacheStorageService.Verify(
                x => x.SaveToCache(apiResponse.Id.ToString(), It.IsAny<CourseDemand>(),
                    TimeSpan.FromMinutes(30)), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_added_To_The_Cache_If_If_Not_Already_Created(
            Guid id,
            string locationName,
            GetRestartCourseDemandResponse apiResponse,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiResponse.RestartDemandExists = false;
            apiClient.Setup(x =>
                    x.Get<GetRestartCourseDemandResponse>(
                        It.Is<GetRestartCourseDemandRequest>(c => c.GetUrl.Contains(id.ToString()))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetRestartCourseDemand(id);
            
            //Assert
            actual.Id.Should().Be(apiResponse.Id);
            actual.EmailVerified.Should().Be(apiResponse.EmailVerified);
            actual.RestartDemandExists.Should().Be(apiResponse.RestartDemandExists);
            cacheStorageService.Verify(
                x => x.SaveToCache(apiResponse.Id.ToString(), It.Is<CourseDemand>(c => 
                        c.Id.Equals(apiResponse.Id)
                        && !c.EmailVerified
                        && c.Location.Equals(apiResponse.Location.Name)
                        && c.OrganisationName.Equals(apiResponse.OrganisationName)
                        && c.ContactEmailAddress.Equals(apiResponse.ContactEmail)
                        && c.NumberOfApprentices.Equals(apiResponse.NumberOfApprentices.ToString())
                        && c.TrainingCourseId.Equals(apiResponse.Course.Id)
                    ),
                    TimeSpan.FromMinutes(30)), Times.Once);
        }
        

    }
}