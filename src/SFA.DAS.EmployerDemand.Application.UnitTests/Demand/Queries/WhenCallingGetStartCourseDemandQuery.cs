using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Queries
{
    public class WhenCallingGetStartCourseDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Data_Returned(
            GetStartCourseDemandQuery query,
            GetStartCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            GetStartCourseDemandQueryHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetStartCourseDemand(query.TrainingCourseId))
                .ReturnsAsync(response);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Course.Should().BeEquivalentTo(response.Course);
            actual.EntryPoint.Should().Be(query.EntryPoint);
        }
    }
}