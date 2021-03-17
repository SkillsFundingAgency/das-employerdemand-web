using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Locations.Services;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Locations.Services
{
    public class WhenGettingLocations
    {
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_The_Api(
            string searchTerm,
            string baseUrl,
            Domain.Locations.Api.Responses.Locations apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            LocationService service)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<Domain.Locations.Api.Responses.Locations>(
                        It.Is<GetLocationsApiRequest>(c => c.GetUrl.Contains(searchTerm))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await service.GetLocations(searchTerm);
            
            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}