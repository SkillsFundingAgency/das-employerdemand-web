using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.RestartEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenHandlingRestartEmployerDemandCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Service_Called(
            RestartEmployerDemandCommand command,
            RestartCourseDemand serviceResponse,
            [Frozen] Mock<IDemandService> service,
            RestartEmployerDemandCommandHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetRestartCourseDemand(command.EmployerDemandId)).ReturnsAsync(serviceResponse);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.Id.Should().Be(serviceResponse.Id);
            actual.EmailVerified.Should().Be(serviceResponse.EmailVerified);
            actual.RestartDemandExists.Should().Be(serviceResponse.RestartDemandExists);
            actual.TrainingCourseId.Should().Be(serviceResponse.TrainingCourseId);
        }

    }
}