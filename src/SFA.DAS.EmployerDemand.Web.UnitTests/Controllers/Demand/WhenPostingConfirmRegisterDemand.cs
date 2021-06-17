using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenPostingConfirmRegisterDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_With_Id_And_Encoded_Id_And_Redirected_To_ConfirmEmail(
            Guid demandId,
            int trainingCourseId,
            string verifyUrl,
            string stopSharingUrl,
            string startSharingUrl,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            UrlRouteContext verifyRouteValues = null;
            UrlRouteContext stopSharingRouteValues = null;
            UrlRouteContext startSharingRouteValues = null;
            var httpContext = new DefaultHttpContext();
            var toEncode = WebEncoders.Base64UrlDecode(demandId.ToString());
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>
                    c.RouteName.Equals(RouteNames.RegisterDemandCompleted)
                )))
                .Returns(verifyUrl)
                .Callback<UrlRouteContext>(c =>
                {
                    verifyRouteValues = c;
                });
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>
                    c.RouteName.Equals(RouteNames.StoppedInterest)
                )))
                .Returns(stopSharingUrl)
                .Callback<UrlRouteContext>(c =>
                {
                    stopSharingRouteValues = c;
                });
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>
                    c.RouteName.Equals(RouteNames.RestartInterest)
                )))
                .Returns(startSharingUrl)
                .Callback<UrlRouteContext>(c =>
                {
                    startSharingRouteValues = c;
                });
            provider.Setup(x => x.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName)).Returns(protector.Object);
            protector.Setup(c => c.Protect(It.Is<byte[]>(
                x => x[0].Equals(Encoding.UTF8.GetBytes($"{demandId}")[0])))).Returns(toEncode);
            controller.Url = urlHelper.Object;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            
            //Act
            var actual = await controller.PostConfirmRegisterDemand(trainingCourseId, demandId) as RedirectToRouteResult;
            
            //Assert
            mediator.Verify(x =>
                x.Send(It.Is<CreateCourseDemandCommand>(c => 
                        c.Id == demandId
                        && c.ResponseUrl == verifyUrl
                        && c.StopSharingUrl == stopSharingUrl
                        && c.StartSharingUrl == startSharingUrl
                    ), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ConfirmEmployerDemandEmail);
            actual.RouteValues["id"].Should().Be(trainingCourseId);
            actual.RouteValues["createDemandId"].Should().Be(demandId);
            verifyRouteValues.Values.Should().BeEquivalentTo(new
            {
                id = trainingCourseId,
                demandId = WebEncoders.Base64UrlEncode(toEncode)
            });
            stopSharingRouteValues.Values.Should().BeEquivalentTo(new
            {
                demandId = WebEncoders.Base64UrlEncode(toEncode)
            });
            startSharingRouteValues.Values.Should().BeEquivalentTo(new
            {
                demandId = WebEncoders.Base64UrlEncode(toEncode)
            });
        }
    }
}