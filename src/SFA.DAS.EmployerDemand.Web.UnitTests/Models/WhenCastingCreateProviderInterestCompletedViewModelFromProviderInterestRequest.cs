using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingCreateProviderInterestCompletedViewModelFromProviderInterestRequest
    {
        [Test, AutoData]
        public void Then_Maps_Fields(ProviderInterestRequest source)
        {
            var result = (CreateProviderInterestCompletedViewModel) source;

            result.Should().BeEquivalentTo(source, options => options
                .ExcludingMissingMembers());
        }
    }
}