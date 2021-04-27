using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand
{
    public class WhenCastingFromApiProviderContactDetailsToProviderContactDetails
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ProviderContactDetails source)
        {
            //Act
            var actual = (Domain.Demand.ProviderContactDetails) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            //Act
            var actual = (Domain.Demand.ProviderContactDetails)(ProviderContactDetails) null;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}