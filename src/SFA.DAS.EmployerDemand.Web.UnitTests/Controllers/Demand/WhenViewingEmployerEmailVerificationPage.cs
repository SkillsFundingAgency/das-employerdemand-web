using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnverifiedEmployerCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenViewingEmployerEmailVerificationPage
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_The_Unverified_Demand_Returned(
            int courseId,
            Guid demandId,
            GetUnverifiedEmployerCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.CourseDemand.EmailVerified = false;
            mediator.Setup(x =>
                    x.Send(It.Is<GetUnverifiedEmployerCourseDemandQuery>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.VerifyEmployerDemandEmail(courseId, demandId) as ViewResult;
            
            //Assert
            actual.Should().NotBeNull();
            var actualModel = actual.Model as VerifyEmployerCourseDemandViewModel;
            actualModel.Should().NotBeNull();
            actualModel.Verified.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Interest_Does_Not_Exist_Then_Redirected_To_Start_Create_Interest(
            int courseId,
            Guid demandId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediator.Setup(x =>
                    x.Send(It.Is<GetUnverifiedEmployerCourseDemandQuery>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetUnverifiedEmployerCourseDemandQueryResult{CourseDemand = null});
            
            //Act
            var actual = await controller.VerifyEmployerDemandEmail(courseId, demandId) as RedirectToRouteResult;
            
            //Assert
            actual.Should().NotBeNull();
            actual.RouteName.Should().Be(RouteNames.StartRegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
        }

        [Test, MoqAutoData]
        public async Task And_Demand_Has_Been_Anonymised_Then_Redirect_To_Restart_Demand(
            int courseId,
            Guid demandId,
            GetUnverifiedEmployerCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.CourseDemand.ContactEmailAddress = string.Empty;
            var toEncode = WebEncoders.Base64UrlDecode(demandId.ToString());
            provider.Setup(x => x.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName)).Returns(protector.Object);
            protector.Setup(c => c.Protect(It.Is<byte[]>(
                x => x[0].Equals(Encoding.UTF8.GetBytes($"{demandId}")[0])))).Returns(toEncode);
            mediatorResult.CourseDemand.EmailVerified = true;
            mediator.Setup(x =>
                    x.Send(It.Is<GetUnverifiedEmployerCourseDemandQuery>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.VerifyEmployerDemandEmail(courseId, demandId) as RedirectToRouteResult;
            
            //Assert
            actual.Should().NotBeNull();
            actual.RouteName.Should().Be(RouteNames.RestartInterest);
            actual.RouteValues.ContainsKey("demandId").Should().BeTrue();
            actual.RouteValues["demandId"].Should().Be(WebEncoders.Base64UrlEncode(toEncode));
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Interest_Is_Already_Verified_Then_Redirected_To_Complete(
            int courseId,
            Guid demandId,
            GetUnverifiedEmployerCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            var toEncode = WebEncoders.Base64UrlDecode(demandId.ToString());
            provider.Setup(x => x.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName)).Returns(protector.Object);
            protector.Setup(c => c.Protect(It.Is<byte[]>(
                x => x[0].Equals(Encoding.UTF8.GetBytes($"{demandId}")[0])))).Returns(toEncode);
            mediatorResult.CourseDemand.EmailVerified = true;
            mediator.Setup(x =>
                    x.Send(It.Is<GetUnverifiedEmployerCourseDemandQuery>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.VerifyEmployerDemandEmail(courseId, demandId) as RedirectToRouteResult;
            
            //Assert
            actual.Should().NotBeNull();
            actual.RouteName.Should().Be(RouteNames.RegisterDemandCompleted);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
            actual.RouteValues.ContainsKey("demandId").Should().BeTrue();
            actual.RouteValues["demandId"].Should().Be(WebEncoders.Base64UrlEncode(toEncode));
        }
    }
}