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
            GetCourseDemandResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiResponse.EmailVerified = true;
            apiClient.Setup(x =>
                    x.Get<GetCourseDemandResponse>(It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{employerDemandKey}"))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetUnverifiedEmployerCourseDemand(employerDemandKey);

            //Assert
            actual.ContactEmailAddress.Should().Be(apiResponse.ContactEmail);
            actual.Id.Should().Be(apiResponse.Id);
            actual.EmailVerified.Should().Be(apiResponse.EmailVerified);

        }
        
        [Test, MoqAutoData]
        public async Task Then_If_It_Does_Not_Exist_On_The_Api_Null_Returned(
            Guid employerDemandKey,
            CourseDemand item,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
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