using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenGettingCreateCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Data_Returned(
            int trainingCourseId,
            GetCreateCourseDemandResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiClient.Setup(x =>
                x.Get<GetCreateCourseDemandResponse>(
                    It.Is<GetCreateDemandRequest>(c => c.GetUrl.Contains(trainingCourseId.ToString()))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetCreateCourseDemand(trainingCourseId);
            
            //Assert
            actual.Should().BeEquivalentTo(apiResponse.Course);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Course_Then_Null_Is_Returned(
            int trainingCourseId,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<GetCreateCourseDemandResponse>(
                        It.Is<GetCreateDemandRequest>(c => c.GetUrl.Contains(trainingCourseId.ToString()))))
                .ReturnsAsync((GetCreateCourseDemandResponse)null);
            
            //Act
            var actual = await service.GetCreateCourseDemand(trainingCourseId);
            
            //Assert
            actual.Should().BeNull();
        }
    }
}