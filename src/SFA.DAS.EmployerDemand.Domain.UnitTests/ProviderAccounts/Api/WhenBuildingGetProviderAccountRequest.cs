using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.ProviderAccounts.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.ProviderAccounts.Api;

public class WhenBuildingGetProviderAccountRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int ukprn)
    {
        var actual = new GetProviderAccountRequest(ukprn);

        actual.GetUrl.Should().Be($"ProviderAccounts/{ukprn}");
    }
}