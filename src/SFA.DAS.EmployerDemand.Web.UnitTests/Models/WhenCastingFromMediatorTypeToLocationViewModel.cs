using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Locations;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromMediatorTypeToLocationViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(Location source)
        {
            //Act
            var actual = (LocationViewModel) source;

            //Assert
            source.Should().BeEquivalentTo(actual);
        }
    }
}