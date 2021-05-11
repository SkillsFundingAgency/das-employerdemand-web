using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromProviderInterestRequestToReviewProviderDetailsViewModel
    {
        [Test, MoqAutoData]
        public void Then_The_Fields_Are_Mapped_Correctly(ProviderInterestRequest source)
        {
            var actual = (ReviewProviderDetailsViewModel) source;

            actual.Should().BeEquivalentTo(source);
            actual.RouteDictionary["ukprn"].Should().Be(source.Ukprn.ToString());
            actual.RouteDictionary["courseId"].Should().Be(source.Course.Id.ToString());
            actual.RouteDictionary["id"].Should().Be(source.Id.ToString());
        }
    }
}