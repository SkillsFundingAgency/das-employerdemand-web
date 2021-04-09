using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Queries
{
    public class WhenCallingGetProviderEmployerDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Data_Returned(
            GetProviderEmployerDemandQuery query,
            GetProviderEmployerDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            GetProviderEmployerDemandQueryHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetProviderEmployerDemand(query.Ukprn, query.CourseId, query.Location, query.LocationRadius))
                .ReturnsAsync(response);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Courses.Should().BeEquivalentTo(response.TrainingCourses);
            actual.TotalFiltered.Should().Be(response.FilteredResults);
            actual.TotalResults.Should().Be(response.TotalResults);
            actual.CourseDemands.Should().BeEquivalentTo(actual.CourseDemands);
            actual.SelectedCourseId.Should().Be(query.CourseId);
        }
    }
}