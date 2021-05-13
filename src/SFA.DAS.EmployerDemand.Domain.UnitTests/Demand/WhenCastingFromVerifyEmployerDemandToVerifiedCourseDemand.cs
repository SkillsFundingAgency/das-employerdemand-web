using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand
{
    public class WhenCastingFromVerifyEmployerDemandToVerifiedCourseDemand
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(VerifyEmployerCourseDemandResponse response)
        {
            //Act 
            var actual = (VerifiedCourseDemand) response;
            
            //Assert
            actual.Should().BeEquivalentTo(response, options => options.Excluding(x=>x.Location));
            actual.Location.Name.Should().Be(response.Location.Name);
            actual.Location.LocationPoint.Should().BeEquivalentTo(response.Location.LocationPoint.GeoPoint);

        }
    }
}