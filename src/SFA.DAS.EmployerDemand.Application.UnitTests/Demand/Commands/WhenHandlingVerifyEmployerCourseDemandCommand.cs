using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenHandlingVerifyEmployerCourseDemandCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Data_Returned(
            VerifiedCourseDemand result,
            VerifyEmployerCourseDemandCommand command,
            [Frozen]Mock<IDemandService> service,
            VerifyEmployerCourseDemandCommandHandler handler)
        {
            //Arrange
            service.Setup(x => x.VerifyEmployerCourseDemand(command.Id)).ReturnsAsync(result);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(result);
        }
    }
}