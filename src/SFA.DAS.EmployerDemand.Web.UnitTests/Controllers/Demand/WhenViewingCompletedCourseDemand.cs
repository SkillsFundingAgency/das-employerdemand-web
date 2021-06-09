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
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenViewingCompletedCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_With_Decoded_Id_And_The_ViewModel_Returned_And_FAT_Url_Taken_From_Config(
            int courseId,
            Guid demandId,
            VerifyEmployerCourseDemandCommandResult mediatorResult,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> config,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            var encodedData = Encoding.UTF8.GetBytes($"{demandId}");
            var id = WebEncoders.Base64UrlEncode(encodedData);
            protector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(encodedData);
            provider.Setup(x => x.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName)).Returns(protector.Object);
            mediator.Setup(x =>
                    x.Send(It.Is<VerifyEmployerCourseDemandCommand>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.RegisterDemandCompleted(courseId, id) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as CompletedCourseDemandViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.TrainingCourse.Should().BeEquivalentTo(mediatorResult.EmployerDemand.Course);
            actualModel.FindApprenticeshipTrainingCourseUrl.Should().Be(config.Object.Value.FindApprenticeshipTrainingUrl + "/courses");
        }


        [Test, MoqAutoData]
        public async Task Then_If_The_Command_Result_Is_Null_Then_Redirect_To_Start_Demand(
            int courseId,
            Guid demandId,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            VerifyEmployerCourseDemandCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            var encodedData = Encoding.UTF8.GetBytes($"{demandId}");
            var id = WebEncoders.Base64UrlEncode(encodedData);
            protector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(encodedData);
            provider.Setup(x => x.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName)).Returns(protector.Object);
            mediatorResult.EmployerDemand = null;
            mediator.Setup(x =>
                    x.Send(It.Is<VerifyEmployerCourseDemandCommand>(c => 
                            c.Id.Equals(demandId))
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.RegisterDemandCompleted(courseId, id) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            
            actual.RouteName.Should().Be(RouteNames.StartRegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Is_Not_A_Guid_Then_Redirect_To_StartRegisterDemand(
            int courseId,
            string demandId,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Greedy] RegisterDemandController controller)
        {
            var encodedData = Encoding.UTF8.GetBytes($"{demandId}");
            var id = WebEncoders.Base64UrlEncode(encodedData);
            protector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(encodedData);
            provider.Setup(x => x.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName)).Returns(protector.Object);
            
            //Act
            var actual = await controller.RegisterDemandCompleted(courseId, id) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            
            actual.RouteName.Should().Be(RouteNames.StartRegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Can_Not_Be_Decoded_Then_Redirect_To_StartRegisterDemand(
            int courseId,
            string demandId,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Greedy] RegisterDemandController controller)
        {
            var encodedData = Encoding.UTF8.GetBytes($"{demandId}");
            var id = WebEncoders.Base64UrlEncode(encodedData);
            protector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Throws<CryptographicException>();
            provider.Setup(x => x.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName)).Returns(protector.Object);
            
            //Act
            var actual = await controller.RegisterDemandCompleted(courseId, id) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            
            actual.RouteName.Should().Be(RouteNames.StartRegisterDemand);
            actual.RouteValues.ContainsKey("Id").Should().BeTrue();
            actual.RouteValues["Id"].Should().Be(courseId);
        }
    }
}