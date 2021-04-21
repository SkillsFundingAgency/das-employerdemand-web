using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand
{
    public class WhenCastingFromProviderEmployerDemandDetailsItemToCourseDemandDetails
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(ProviderEmployerDemandDetailsItem source)
        {
            //Act
            var actual = (ProviderCourseDemandDetails) source;
            
            //Assert
            actual.DemandId.Should().Be(source.DemandId);
            actual.NumberOfApprentices.Should().Be(source.Apprentices);
            actual.Location.Should().BeEquivalentTo((Location)source.Location);
        }
    }
}