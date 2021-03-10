using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Locations.Api;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Locations.Api
{
    public class WhenBuildingTheGetLocationsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string searchTerm)
        {
            //Arrange Act
            var actual = new GetLocationsApiRequest(searchTerm);
            
            //Assert
            actual.GetUrl.Should().Be($"locations?searchTerm={searchTerm}");
        }
    }
}