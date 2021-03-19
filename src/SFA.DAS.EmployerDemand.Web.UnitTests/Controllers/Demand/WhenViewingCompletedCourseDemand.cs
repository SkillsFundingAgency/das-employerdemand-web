using System;
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
    public class WhenViewingCompletedCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_The_ViewModel_Returned_And_FAT_Url_Taken_From_Config(
            int courseId,
            Guid demandId,
            GetCachedCreateCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> config,
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
            var actual = await controller.RegisterDemandCompleted(courseId, demandId) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as CompletedCourseDemandViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.TrainingCourse.Should().BeEquivalentTo(mediatorResult.CourseDemand.Course);
            actualModel.FindApprenticeshipTrainingCourseUrl.Should().Be(config.Object.Value.FindApprenticeshipTrainingUrl + "/courses");
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
            var actual = await controller.RegisterDemandCompleted(courseId, demandId) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            
            actual.RouteName.Should().Be(RouteNames.RegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
        }
    }
}