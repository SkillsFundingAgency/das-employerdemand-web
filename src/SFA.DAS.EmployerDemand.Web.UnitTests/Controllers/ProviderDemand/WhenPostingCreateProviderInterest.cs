using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateProviderInterest;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenPostingCreateProviderInterest
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_Redirected_To_Completed(
            CreateProviderInterestRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Act
            var actual = await controller.PostCreateProviderInterest(request) as RedirectToRouteResult;
            
            //Assert
            mockMediator.Verify(mediator =>
                mediator.Send(
                    It.Is<CreateProviderInterestCommand>(command => command.Id == request.Id),
                    It.IsAny<CancellationToken>()), 
                Times.Once);
            
            actual!.RouteName.Should().Be(RouteNames.CreateProviderInterestCompleted);
            actual.RouteValues["ukprn"].Should().Be(request.Ukprn);
            actual.RouteValues["courseId"].Should().Be(request.CourseId);
            actual.RouteValues["id"].Should().Be(request.Id);
        }

        /*[Test, MoqAutoData]
        public async Task And_No_Model_From_Mediator_Then_Redirect_To_ProviderDemandDetails(
            CreateProviderInterestRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateProviderInterestCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new );

            //Act
            var actual = await controller.PostCreateProviderInterest(request) as RedirectToRouteResult;
            
            //Assert
            mockMediator.Verify(mediator =>
                    mediator.Send(
                        It.Is<CreateProviderInterestCommand>(command => command.Id == request.Id),
                        It.IsAny<CancellationToken>()), 
                Times.Once);
            
            actual!.RouteName.Should().Be(RouteNames.CreateProviderInterestCompleted);
            actual.RouteValues["ukprn"].Should().Be(request.Ukprn);
            actual.RouteValues["courseId"].Should().Be(request.CourseId);
            actual.RouteValues["id"].Should().Be(request.Id);
        }

        //todo and no cache found return to start of flow*/
    }
}