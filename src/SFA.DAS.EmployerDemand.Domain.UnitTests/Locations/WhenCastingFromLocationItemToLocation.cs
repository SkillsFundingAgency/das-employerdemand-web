using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Locations;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Locations
{
    public class WhenCastingFromLocationItemToLocation
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(LocationItem source)
        {
            //Act
            var actual = (Location) source;
            
            //Assert
            actual.Name.Should().Be(source.Name);
            actual.LocationPoint.Should().BeEquivalentTo(source.LocationPoint.GeoPoint);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            //Act
            var actual = (Location) (LocationItem) null;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}