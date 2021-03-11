using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands;
using SFA.DAS.EmployerDemand.Application.Demand.Queries;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenCreatingRegisterDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_Redirected_To_Confirm(
            RegisterDemandRequest request,
            CreateCachedCourseDemandCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediator.Setup(x =>
                    x.Send(It.Is<CreateCourseDemandCommand>(c => 
                            c.TrainingCourseId.Equals(request.TrainingCourseId)
                            && c.Location.Equals(request.Location)
                            && c.OrganisationName.Equals(request.OrganisationName)
                            && c.ContactEmailAddress.Equals(request.ContactEmailAddress)
                            && c.NumberOfApprentices.Equals(request.NumberOfApprentices)
                            && c.Id != Guid.Empty
                            )
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.PostRegisterDemand(request) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ConfirmRegisterDemand);
            actual.RouteValues["id"].Should().Be(mediatorResult.Id);
        }

        [Test, MoqAutoData]
        public async Task Then_Is_ModelState_Error_Then_Command_Is_Not_Called_And_Register_View_Returned(
            RegisterDemandRequest request,
            CreateCachedCourseDemandCommandResult mediatorResult,
            GetCreateCourseDemandQueryResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            controller.ModelState.AddModelError("key", "error message");
            mediator.Setup(x =>
                    x.Send(It.Is<GetCreateCourseDemandQuery>(c => c.TrainingCourseId.Equals(request.TrainingCourseId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostRegisterDemand(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("RegisterDemand");
            mediator.Verify(x =>
                x.Send(It.IsAny<CreateCourseDemandCommand>()
                    , It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Validation_Exception_The_Register_View_Is_Returned(
            RegisterDemandRequest request,
            CreateCachedCourseDemandCommandResult mediatorResult,
            GetCreateCourseDemandQueryResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediator.Setup(x =>
                    x.Send(It.IsAny<CreateCourseDemandCommand>()
                        , It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException());
            mediator.Setup(x =>
                    x.Send(It.Is<GetCreateCourseDemandQuery>(c => c.TrainingCourseId.Equals(request.TrainingCourseId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostRegisterDemand(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("RegisterDemand");
        }
    }
}