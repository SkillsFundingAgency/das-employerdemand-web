using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenGettingTheDashboard
    {
        [Test, MoqAutoData]
        public void Then_Returned_Redirected_To_SharedUiDashboard(
            string redirectUrl,
            [Frozen] Mock<IOptions<ProviderSharedUIConfiguration>> mockOptions,
            [Greedy] HomeController controller)
        {
            //Arrange
            mockOptions.Object.Value.DashboardUrl = redirectUrl;

            //Act
            var actual = controller.Dashboard() as RedirectResult;

            //Assert
            Assert.IsNotNull(actual);
            actual.Url.Should().Be(redirectUrl);
        }
    }
}
