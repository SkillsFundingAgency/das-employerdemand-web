using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingThePostCreateProviderInterestsData
    {
        [Test, MoqAutoData]
        public void Then_Maps_Fields(
            IProviderDemandInterest interest,
            string fatUrl)
        {
            string BuildFatUrl(IProviderDemandInterest providerDemandInterest) => fatUrl;

            var data = new PostCreateProviderInterestsData(interest, BuildFatUrl);

            data.Id.Should().Be(interest.Id);
            data.EmployerDemandIds.Should()
                .BeEquivalentTo(interest.EmployerDemands
                    .Select(demands => demands.EmployerDemandId));
            data.Ukprn.Should().Be(interest.Ukprn);
            data.ProviderName.Should().Be(interest.ProviderName);
            data.Email.Should().Be(interest.EmailAddress);
            data.Phone.Should().Be(interest.PhoneNumber);
            data.Website.Should().Be(interest.Website);
            data.FatUrl.Should().Be(fatUrl);
        }
    }
}