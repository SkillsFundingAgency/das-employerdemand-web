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
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCreateCourseDemand;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenViewingRegisterDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_The_ViewModel_Returned(
            int trainingCourseId,
            short? entryPoint,
            GetCreateCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.CourseDemand.Course.LastStartDate = DateTime.Today.AddDays(1);
            mediator.Setup(x =>
                    x.Send(It.Is<GetCreateCourseDemandQuery>(c => 
                        c.TrainingCourseId.Equals(trainingCourseId)
                        && c.CreateDemandId == null
                        && c.EntryPoint.Equals(entryPoint))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.RegisterDemand(trainingCourseId, null, entryPoint) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as RegisterCourseDemandViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.TrainingCourse.Should().BeEquivalentTo(mediatorResult.CourseDemand.Course);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_CreateDemand_Id_Then_It_Is_Passed_To_The_Query(
            int trainingCourseId,
            Guid createDemandId,
            short? entryPoint,
            GetCreateCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.CourseDemand.Course.LastStartDate = DateTime.Today.AddDays(1);
            mediator.Setup(x =>
                    x.Send(It.Is<GetCreateCourseDemandQuery>(c => 
                            c.TrainingCourseId.Equals(trainingCourseId)
                            && c.CreateDemandId.Equals(createDemandId)
                            && c.EntryPoint == null)
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.RegisterDemand(trainingCourseId, createDemandId, null) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as RegisterCourseDemandViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.TrainingCourse.Should().BeEquivalentTo(mediatorResult.CourseDemand.Course);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Course_Has_Expired_Redirected_To_FindApprenticeshipTraining(
            int trainingCourseId,
            string baseUrl,
            GetCreateCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> config,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            config.Object.Value.FindApprenticeshipTrainingUrl = baseUrl;
            mediatorResult.CourseDemand.Course.LastStartDate = DateTime.Today.AddDays(-1);
            mediator.Setup(x =>
                    x.Send(It.Is<GetCreateCourseDemandQuery>(c =>
                            c.TrainingCourseId.Equals(trainingCourseId)
                            && c.CreateDemandId == null)
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            //Act
            var actual = await controller.RegisterDemand(trainingCourseId, null, null) as RedirectResult;

            //Assert
            Assert.IsNotNull(actual);
            actual.Url.Should().Be($"{baseUrl}/courses/{mediatorResult.CourseDemand.Course.Id}");
            actual.Permanent.Should().BeFalse();
            actual.PreserveMethod.Should().BeTrue();
        }
    }
}