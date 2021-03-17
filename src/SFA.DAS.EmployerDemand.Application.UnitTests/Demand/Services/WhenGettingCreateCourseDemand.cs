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
            string locationName,
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
            var actual = await service.GetCreateCourseDemand(trainingCourseId, locationName);
            
            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Course_Or_Location_It_Is_Set_To_Null(
            int trainingCourseId,
            string locationName,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<GetCreateCourseDemandResponse>(
                        It.Is<GetCreateDemandRequest>(c => c.GetUrl.Contains(trainingCourseId.ToString()))))
                .ReturnsAsync(new GetCreateCourseDemandResponse());
            
            //Act
            var actual = await service.GetCreateCourseDemand(trainingCourseId, locationName);
            
            //Assert
            actual.Course.Should().BeNull();
            actual.Location.Should().BeNull();
        }
    }
}