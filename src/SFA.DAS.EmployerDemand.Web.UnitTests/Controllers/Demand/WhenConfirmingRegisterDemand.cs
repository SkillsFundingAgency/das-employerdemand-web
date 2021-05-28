using System;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
            mediatorResult.CourseDemand.Course.LastStartDate = DateTime.Now.AddDays(1);
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
        public async Task Then_If_The_Cached_Object_Is_Null_Then_Redirect_To_StartRegisterDemand(
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
            
            actual.RouteName.Should().Be(RouteNames.StartRegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
        }

        [Test, MoqAutoData]
        public async Task
            Then_If_The_Course_LastStartDate_Is_In_The_Past_Then_Redirected_To_FindApprenticeshipTraining_Course_List(
                int courseId,
                Guid demandId,
                string baseUrl,
                GetCachedCreateCourseDemandQueryResult mediatorResult,
                [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> config,
                [Frozen] Mock<IMediator> mediator,
                [Greedy] RegisterDemandController controller)
        {
            //Arrange
            config.Object.Value.FindApprenticeshipTrainingUrl = baseUrl;
            mediatorResult.CourseDemand.Course.LastStartDate = DateTime.Today.AddDays(-1);
            mediator.Setup(x =>
                    x.Send(It.Is<GetCachedCreateCourseDemandQuery>(c =>
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            //Act
            var actual = await controller.ConfirmRegisterDemand(courseId, demandId) as RedirectResult;

            //Assert
            Assert.IsNotNull(actual);
            actual.Url.Should().Be($"{baseUrl}courses/{mediatorResult.CourseDemand.Course.Id}");
            actual.Permanent.Should().BeFalse();
            actual.PreserveMethod.Should().BeTrue();
        }
    }
}