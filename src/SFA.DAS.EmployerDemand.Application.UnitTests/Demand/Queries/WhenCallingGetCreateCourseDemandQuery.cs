using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCreateCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Queries
{
    public class WhenCallingGetCreateCourseDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Data_Returned(
            GetCreateCourseDemandQuery query,
            GetCreateCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            GetCreateCourseDemandQueryHandler handler)
        {
            //Arrange
            query.CreateDemandId = null;
            service.Setup(x => x.GetCreateCourseDemand(query.TrainingCourseId, ""))
                .ReturnsAsync(response);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.CourseDemand.Course.Should().BeEquivalentTo(response.Course);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_CreateDemandId_Then_It_Is_Read_From_The_Cache(
            GetCreateCourseDemandQuery query,
            CourseDemandRequest cachedData,
            GetCreateCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            GetCreateCourseDemandQueryHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetCachedCourseDemand(query.CreateDemandId.Value)).ReturnsAsync(cachedData);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            service.Verify(x => x.GetCreateCourseDemand(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_CreateDemandId_And_The_Cached_Value_Is_Null_Then_The_Service_Is_Called(
            GetCreateCourseDemandQuery query,
            GetCreateCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            GetCreateCourseDemandQueryHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetCachedCourseDemand(query.CreateDemandId.Value)).ReturnsAsync((CourseDemandRequest)null);
            service.Setup(x => x.GetCreateCourseDemand(query.TrainingCourseId, ""))
                .ReturnsAsync(response);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.CourseDemand.Course.Should().BeEquivalentTo(response.Course);
        }
    }
}