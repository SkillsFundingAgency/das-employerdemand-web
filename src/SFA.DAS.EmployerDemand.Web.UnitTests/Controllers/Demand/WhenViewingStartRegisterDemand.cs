using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    }
}