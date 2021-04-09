using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand
{
    public class WhenCastingFromProviderEmployerDemandItemToCourseDemand
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(ProviderEmployerDemandItem source)
        {
            //Act
            var actual = (ProviderCourseDemand) source;
            
            //Assert
            actual.Course.Should().BeEquivalentTo(source.TrainingCourse);
            actual.NumberOfApprentices.Should().Be(source.Apprentices);
            actual.NumberOfEmployers.Should().Be(source.Employers);
        }
    }
}