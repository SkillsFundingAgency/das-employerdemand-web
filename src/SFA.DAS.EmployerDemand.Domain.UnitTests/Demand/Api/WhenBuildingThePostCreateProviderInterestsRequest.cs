using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingThePostCreateProviderInterestsRequest
    {
        [Test, MoqAutoData]
        public void Then_Url_Correct_And_Data_Populated(
            PostCreateProviderInterestsData data)
        {
            var request = new PostCreateProviderInterestsRequest(data);

            request.PostUrl.Should().Be($"providerinterest");
            request.Data.Should().Be(data);
        }
    }
}