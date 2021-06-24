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
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenViewingStartRegisterDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_The_ViewModel_Returned(
            int trainingCourseId,
            GetStartCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.Course.LastStartDate = DateTime.Today.AddDays(1);
            mediator.Setup(x =>
                    x.Send(It.Is<GetStartCourseDemandQuery>(c => 
                            c.TrainingCourseId.Equals(trainingCourseId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.StartRegisterDemand(trainingCourseId) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as StartRegisterCourseDemandViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.TrainingCourse.Should().BeEquivalentTo(mediatorResult.Course);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Course_Has_Expired_Redirected_To_FindApprenticeshipTraining(
            int trainingCourseId,
            string baseUrl,
            GetStartCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> config,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            config.Object.Value.FindApprenticeshipTrainingUrl = baseUrl;
            mediatorResult.Course.LastStartDate = DateTime.Today.AddDays(-1);
            mediator.Setup(x =>
                    x.Send(It.Is<GetStartCourseDemandQuery>(c =>
                            c.TrainingCourseId.Equals(trainingCourseId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            //Act
            var actual = await controller.StartRegisterDemand(trainingCourseId) as RedirectResult;

            //Assert
            Assert.IsNotNull(actual);
            actual.Url.Should().Be($"{baseUrl}/courses/{mediatorResult.Course.Id}");
            actual.Permanent.Should().BeFalse();
            actual.PreserveMethod.Should().BeTrue();
        }
    }
}