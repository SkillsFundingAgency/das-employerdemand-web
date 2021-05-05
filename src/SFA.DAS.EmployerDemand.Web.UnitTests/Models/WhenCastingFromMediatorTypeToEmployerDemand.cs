using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromMediatorTypeToEmployerDemand
    {
        [Test, MoqAutoData]
        public void Then_The_Fields_Are_Mapped_Correctly(Domain.Demand.EmployerDemands source)
        {
            var actual = (Web.Models.EmployerDemands) source;

            actual.Should().BeEquivalentTo(source);
        }
    }
}