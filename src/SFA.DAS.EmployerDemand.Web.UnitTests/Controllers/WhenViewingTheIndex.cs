using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Web.Controllers;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers
{
    public class WhenViewingTheIndex
    {
        [Test]
        public void Then_The_View_Is_Returned()
        {
            var controller = new HomeController();
            
            var actual = controller.Index() as ViewResult;

            Assert.IsNotNull(actual);
            actual.ViewName.Should().BeNull();
        }
    }
}