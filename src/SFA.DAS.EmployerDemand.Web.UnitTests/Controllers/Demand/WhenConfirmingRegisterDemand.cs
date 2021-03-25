using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenConfirmingRegisterDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_The_ViewModel_Returned(
            int courseId,
            Guid demandId,
            GetCachedCreateCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediator.Setup(x =>
                    x.Send(It.Is<GetCachedCreateCourseDemandQuery>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.ConfirmRegisterDemand(courseId, demandId) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as ConfirmCourseDemandViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.TrainingCourse.Should().BeEquivalentTo(mediatorResult.CourseDemand.Course);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Cached_Object_Is_Null_Then_Redirect_To_EnterApprenticeshipDetails(
            int courseId,
            Guid demandId,
            GetCachedCreateCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.CourseDemand = null;
            mediator.Setup(x =>
                    x.Send(It.Is<GetCachedCreateCourseDemandQuery>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.ConfirmRegisterDemand(courseId, demandId) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            
            actual.RouteName.Should().Be(RouteNames.RegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
        }
    }
}