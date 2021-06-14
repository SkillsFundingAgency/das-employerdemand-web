using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand
{
    public class WhenCastingFromStopEmployerCourseDemandResponseToDomainType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(StopEmployerCourseDemandResponse source)
        {
            //Act
            var actual = (StoppedCourseDemand) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}