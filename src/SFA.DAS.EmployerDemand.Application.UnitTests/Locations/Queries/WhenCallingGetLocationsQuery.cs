using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Locations.Queries;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Locations.Queries
{
    public class WhenCallingGetLocationsQuery
    {
 
        [Test, MoqAutoData]
        public async Task Then_Returns_Results_From_Service(
            GetLocationsQuery query,
            Domain.Locations.Locations locationsFromService,
            [Frozen] Mock<ILocationService> mockService,
            GetLocationsQueryHandler handler)
        {
            mockService
                .Setup(service => service.GetLocations(query.SearchTerm))
                .ReturnsAsync(locationsFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.LocationItems.Should().BeEquivalentTo(locationsFromService.LocationItems);
            
        }       
    }
}