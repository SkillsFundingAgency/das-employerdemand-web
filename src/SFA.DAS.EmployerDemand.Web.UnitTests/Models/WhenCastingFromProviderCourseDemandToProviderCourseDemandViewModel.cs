using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromProviderCourseDemandToProviderCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ProviderCourseDemand source)
        {
            //Act
            var actual = (ProviderCourseDemandViewModel) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}