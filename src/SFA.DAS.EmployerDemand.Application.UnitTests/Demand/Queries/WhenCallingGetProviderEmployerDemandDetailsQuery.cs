using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Queries
{
    public class WhenCallingGetProviderEmployerDemandDetailsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Data_Returned(
            GetProviderEmployerDemandDetailsQuery query,
            GetProviderEmployerDemandDetailsResponse response,
            [Frozen] Mock<IDemandService> service,
            GetProviderEmployerDemandDetailsQueryHandler handler)
        {
            //Arrange
            service
                .Setup(x => x.GetProviderEmployerDemandDetails(query.Ukprn, query.CourseId, query.Location, query.LocationRadius))
                .ReturnsAsync(response);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Course.Should().BeEquivalentTo(response.TrainingCourse);
            actual.CourseDemandDetailsList.Should().BeEquivalentTo(response.ProviderEmployerDemandDetailsList.Select(item => (ProviderCourseDemandDetails)item));
            actual.SelectedLocation.Should().BeEquivalentTo((Location)response.Location);
            actual.SelectedRadius.Should().Be(query.LocationRadius);
            actual.ProviderContactDetails.Should().BeEquivalentTo(response.ProviderContactDetails);
        }
    }
}