using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenGettingStartCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Data_Returned(
            int trainingCourseId,
            string locationName,
            GetStartCourseDemandResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<GetStartCourseDemandResponse>(
                        It.Is<GetStartCourseDemandRequest>(c => c.GetUrl.Contains(trainingCourseId.ToString()))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetStartCourseDemand(trainingCourseId);
            
            //Assert
            actual.Course.Should().BeEquivalentTo(apiResponse.Course);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Course_It_Is_Set_To_Null(
            int trainingCourseId,
            string locationName,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<GetStartCourseDemandResponse>(
                        It.Is<GetStartCourseDemandRequest>(c => c.GetUrl.Contains(trainingCourseId.ToString()))))
                .ReturnsAsync(new GetStartCourseDemandResponse());
            
            //Act
            var actual = await service.GetStartCourseDemand(trainingCourseId);
            
            //Assert
            actual.Course.Should().BeNull();
        }
    }
}