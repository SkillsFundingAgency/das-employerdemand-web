using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenBuildingFatUrl
    {
        [Test, MoqAutoData]
        public void And_Provider_Does_Offer_This_Course_Then_Builds_Url(
            IProviderDemandInterest interest,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            FatUrlBuilderService service)
        {
            interest.ProviderOffersThisCourse = true;

            var fatUrl = service.BuildFatUrl(interest);

            fatUrl.Should().Be($"{mockOptions.Object.Value.FindApprenticeshipTrainingUrl}/courses/{interest.Course.Id}/providers/{interest.Ukprn}");
        }

        [Test, MoqAutoData]
        public void And_Provider_Not_Offer_This_Course_Then_Returns_Null(
            IProviderDemandInterest interest,
            FatUrlBuilderService service)
        {
            interest.ProviderOffersThisCourse = false;

            var fatUrl = service.BuildFatUrl(interest);

            fatUrl.Should().BeNull();
        }
    }
}