using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCourseDemand;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenPostingConfirmRegisterDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_Redirected_To_Completed(
            Guid demandId,
            int trainingCourseId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Act
            var actual = await controller.PostConfirmRegisterDemand(trainingCourseId, demandId) as RedirectToRouteResult;
            
            //Assert
            mediator.Verify(x =>
                x.Send(It.Is<CreateCourseDemandCommand>(c => 
                        c.Id == demandId
                    )
                    , It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.RegisterDemandCompleted);
            actual.RouteValues["id"].Should().Be(trainingCourseId);
            actual.RouteValues["createDemandId"].Should().Be(demandId);
        }
    }
}