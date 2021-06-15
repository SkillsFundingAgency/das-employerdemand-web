using System;
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
            query.Id = null;

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

        [Test, MoqAutoData]
        public async Task Then_If_Cached_Object_The_Service_Is_Called_And_Data_Returned(
            GetProviderEmployerDemandDetailsQuery query,
            GetProviderEmployerDemandDetailsResponse response,
            ProviderInterestRequest providerInterest,
            [Frozen] Mock<IDemandService> service,
            GetProviderEmployerDemandDetailsQueryHandler handler)
        {
            //Arrange
            var expectedDemandIds = providerInterest.EmployerDemands.Select(c => c.EmployerDemandId);

            query.Id = new Guid();
            query.FromLocation = false;
            service.Setup(x => x.GetCachedProviderInterest((Guid)query.Id))
                .ReturnsAsync(providerInterest);

            service
                .Setup(x => x.GetProviderEmployerDemandDetails(query.Ukprn, query.CourseId, query.Location, query.LocationRadius))
                .ReturnsAsync(response);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            service.Verify(c => c.GetCachedProviderInterest(It.IsAny<Guid>()), Times.Once);
            actual.EmployerDemandIds.Should().BeEquivalentTo(expectedDemandIds);
            actual.Id.Should().Be(providerInterest.Id);
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_If_Cached_Object_With_No_Optional_Populated_And_No_Provider_Data_Then_Populated_From_Cache(
            GetProviderEmployerDemandDetailsQuery query,
            GetProviderEmployerDemandDetailsResponse response,
            ProviderInterestRequest providerInterest,
            [Frozen] Mock<IDemandService> service,
            GetProviderEmployerDemandDetailsQueryHandler handler)
        {
            //Arrange
            providerInterest.Website = null;
            response.ProviderContactDetails = null;
            var expectedDemandIds = providerInterest.EmployerDemands.Select(c => c.EmployerDemandId);

            query.Id = new Guid();
            query.FromLocation = false;
            service.Setup(x => x.GetCachedProviderInterest((Guid)query.Id))
                .ReturnsAsync(providerInterest);

            service
                .Setup(x => x.GetProviderEmployerDemandDetails(query.Ukprn, query.CourseId, query.Location, query.LocationRadius))
                .ReturnsAsync(response);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            service.Verify(c => c.GetCachedProviderInterest(It.IsAny<Guid>()), Times.Once);
            actual.EmployerDemandIds.Should().BeEquivalentTo(expectedDemandIds);
            actual.Id.Should().Be(providerInterest.Id);
            actual.ProviderContactDetails.Website.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Cached_Object_The_Service_Is_Not_Called_And_No_Data_Returned(
            GetProviderEmployerDemandDetailsQuery query,
            GetProviderEmployerDemandDetailsResponse response,
            [Frozen] Mock<IDemandService> service,
            GetProviderEmployerDemandDetailsQueryHandler handler)
        {
            //Arrange
            query.Id = null;

            service
                .Setup(x => x.GetProviderEmployerDemandDetails(query.Ukprn, query.CourseId, query.Location, query.LocationRadius))
                .ReturnsAsync(response);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            service.Verify(c => c.GetCachedProviderInterest(It.IsAny<Guid>()), Times.Never);
            actual.EmployerDemandIds.Should().BeEmpty();
            actual.Id.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public async Task Then_If_FromLocation_Then_Employer_Demands_Are_Cleared(
            GetProviderEmployerDemandDetailsQuery query,
            GetProviderEmployerDemandDetailsResponse response,
            ProviderInterestRequest providerInterest,
            [Frozen] Mock<IDemandService> service,
            GetProviderEmployerDemandDetailsQueryHandler handler)
        {
            //Arrange
            query.Id = new Guid();
            query.FromLocation = true;
            service.Setup(x => x.GetCachedProviderInterest((Guid)query.Id))
                .ReturnsAsync(providerInterest);

            service
                .Setup(x => x.GetProviderEmployerDemandDetails(query.Ukprn, query.CourseId, query.Location, query.LocationRadius))
                .ReturnsAsync(response);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.EmployerDemandIds.Should().BeEmpty();
        }
    }
}