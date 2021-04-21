using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromProviderInterestRequestToProviderContactDetailsViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ProviderInterestRequest source)
        {
            //Act
            var actual = (ProviderContactDetailsViewModel) source;
            
            //Assert
            actual.Course.Should().BeEquivalentTo(source.Course);
            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(c=>c.Course)
                .Excluding(c=>c.Ukprn)
                .Excluding(c=>c.EmployerDemandIds)
            );
            
        }
    }
}