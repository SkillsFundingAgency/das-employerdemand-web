using System;
using System.Security.Cryptography;
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
using SFA.DAS.EmployerDemand.Application.Demand.Commands.RestartEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenViewingRestartInterest
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_If_All_Data_Redirected_To_Check_Your_Answers(
            Guid employerDemandId,
            string encodedEmployerDemandId,
            RestartEmployerDemandCommandResult mediatorResult,
            [Frozen] Mock<IDataEncryptDecryptService> dataEncryptDecryptService,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.EmailVerified = false;
            mediatorResult.RestartDemandExists = false;
            mediatorResult.LastStartDate = DateTime.Today.AddDays(1);
            mockMediator
                .Setup(x => x.Send(
                    It.Is<RestartEmployerDemandCommand>(c => c.EmployerDemandId == employerDemandId)
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            dataEncryptDecryptService.Setup(x => x.DecodeData(encodedEmployerDemandId)).Returns(employerDemandId);
            
            //Act
            var actual = await controller.RestartInterest(encodedEmployerDemandId) as RedirectToRouteResult;
            
            //Assert
            actual.RouteName.Should().Be(RouteNames.ConfirmRegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(mediatorResult.TrainingCourseId);
            actual.RouteValues.ContainsKey("createDemandId").Should().BeTrue();
            actual.RouteValues["CreateDemandId"].Should().Be(mediatorResult.Id);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Demand_Already_Created_And_Not_Verified_Then_Redirect_To_VerifyEmail(
            Guid employerDemandId,
            string encodedEmployerDemandId,
            RestartEmployerDemandCommandResult mediatorResult,
            [Frozen] Mock<IDataEncryptDecryptService> dataEncryptDecryptService,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.EmailVerified = false;
            mediatorResult.RestartDemandExists = true;
            mediatorResult.LastStartDate = DateTime.Today.AddDays(1);
            
            mockMediator
                .Setup(x => x.Send(
                    It.Is<RestartEmployerDemandCommand>(c => c.EmployerDemandId == employerDemandId)
                    , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            dataEncryptDecryptService.Setup(x => x.DecodeData(encodedEmployerDemandId)).Returns(employerDemandId);
            
            //Act
            var actual = await controller.RestartInterest(encodedEmployerDemandId) as RedirectToRouteResult;
            
            //Assert
            actual.RouteName.Should().Be(RouteNames.ConfirmEmployerDemandEmail);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(mediatorResult.TrainingCourseId);
            actual.RouteValues.ContainsKey("CreateDemandId").Should().BeTrue();
            actual.RouteValues["CreateDemandId"].Should().Be(mediatorResult.Id);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Demand_Already_Created_And_Verified_Then_Redirect_To_Confirmation(
            Guid employerDemandId,
            string encodedEmployerDemandId,
            RestartEmployerDemandCommandResult mediatorResult,
            [Frozen] Mock<IDataEncryptDecryptService> dataEncryptDecryptService,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mediatorResult.EmailVerified = true;
            mediatorResult.RestartDemandExists = true;
            mediatorResult.LastStartDate = DateTime.Today.AddDays(1);
            
            mockMediator
                .Setup(x => x.Send(
                    It.Is<RestartEmployerDemandCommand>(c => c.EmployerDemandId == employerDemandId)
                    , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            dataEncryptDecryptService.Setup(x => x.DecodeData(encodedEmployerDemandId)).Returns(employerDemandId);
            dataEncryptDecryptService.Setup(x => x.EncodedData(mediatorResult.Id)).Returns(encodedEmployerDemandId);
            
            //Act
            var actual = await controller.RestartInterest(encodedEmployerDemandId) as RedirectToRouteResult;
            
            //Assert
            actual.RouteName.Should().Be(RouteNames.RegisterDemandCompleted);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(mediatorResult.TrainingCourseId);
            actual.RouteValues.ContainsKey("demandId").Should().BeTrue();
            actual.RouteValues["demandId"].Should().Be(encodedEmployerDemandId);
        }

        [Test, MoqAutoData]
        public async Task And_Id_Not_Parseable_Then_Redirect_To_FAT(
            string employerDemandId,
            string encodedEmployerDemandId,
            [Frozen] Mock<IDataEncryptDecryptService> dataEncryptDecryptService,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            dataEncryptDecryptService.Setup(x => x.DecodeData(encodedEmployerDemandId)).Returns((Guid?)null);
            
            //Act
            var actual = await controller.RestartInterest(encodedEmployerDemandId) as RedirectResult;
            
            //Assert
            actual.Url.Should().Be(mockOptions.Object.Value.FindApprenticeshipTrainingUrl);
        }


        [Test, MoqAutoData]
        public async Task Then_If_The_Course_Is_Expired_Redirected_To_FAT(
            Guid employerDemandId,
            string encodedEmployerDemandId,
            string baseUrl,
            RestartEmployerDemandCommandResult mediatorResult,
            [Frozen] Mock<IDataEncryptDecryptService> dataEncryptDecryptService,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            mockOptions.Object.Value.FindApprenticeshipTrainingUrl = baseUrl;
            mediatorResult.LastStartDate = DateTime.Today.AddDays(-1);
            mockMediator
                .Setup(x => x.Send(
                    It.Is<RestartEmployerDemandCommand>(c => c.EmployerDemandId == employerDemandId)
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            dataEncryptDecryptService.Setup(x => x.DecodeData(encodedEmployerDemandId)).Returns(employerDemandId);
            
            //Act
            var actual = await controller.RestartInterest(encodedEmployerDemandId) as RedirectResult;

            //Assert
            Assert.IsNotNull(actual);
            actual.Url.Should().Be($"{baseUrl}/courses/{mediatorResult.TrainingCourseId}");
            actual.Permanent.Should().BeFalse();
            actual.PreserveMethod.Should().BeTrue();
        }
    }
}