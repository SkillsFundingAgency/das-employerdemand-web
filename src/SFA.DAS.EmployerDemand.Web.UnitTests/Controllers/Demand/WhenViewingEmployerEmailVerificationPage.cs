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
            string encodedId,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> config,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediator.Setup(x =>
                    x.Send(It.Is<GetUnverifiedEmployerCourseDemandQuery>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetUnverifiedEmployerCourseDemandQueryResult
                {
                    CourseDemand = null
                });
            
            //Act
            var actual = await controller.VerifyEmployerDemandEmail(courseId, demandId) as RedirectToRouteResult;
            
            //Assert
            actual.Should().NotBeNull();
            actual.RouteName.Should().Be(RouteNames.StartRegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Interest_Is_Already_Verified_Then_Redirected_To_Complete(
            int courseId,
            string encodedId,
            Guid demandId,
            GetUnverifiedEmployerCourseDemandQueryResult mediatorResult,
            [Frozen] Mock<IDataProtectorService> dataEncryptDecryptService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            dataEncryptDecryptService.Setup(x => x.EncodedData(demandId)).Returns(encodedId);
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
            actual.RouteValues["demandId"].Should().Be(encodedId);
        }
    }
}