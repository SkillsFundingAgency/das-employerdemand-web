using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Infrastructure.Authorization;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenGettingTheIndex
    {
        [Test, MoqAutoData]
        public void Then_If_Authenticated_And_Has_Ukprn_Claim_Then_Redirects_To_View(
            int ukprn,
            [Greedy] HomeController controller)
        {
            //Arrange
            var claim = new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString());
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
            controller.ControllerContext = new ControllerContext() {HttpContext = new DefaultHttpContext() { User = claimsPrinciple }};

            //Act
            var actual = controller.Index() as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ProviderDashboard);
            actual.RouteValues["ukprn"].Should().Be(ukprn.ToString());
        }
    }
}