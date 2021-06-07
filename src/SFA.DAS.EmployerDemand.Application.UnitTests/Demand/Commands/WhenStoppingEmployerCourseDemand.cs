using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenStoppingEmployerCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Cached_Item_Is_Returned_And_Service_Called_To_Create(
            StopEmployerCourseDemandCommand command,
            StoppedCourseDemand resultFromService,
            [Frozen] Mock<IDemandService> mockService,
            StopEmployerCourseDemandCommandHandler handler)
        {
            mockService
                .Setup(service => service.StopEmployerCourseDemand(command.EmployerDemandId))
                .ReturnsAsync(resultFromService);

            var result = await handler.Handle(command, CancellationToken.None);

            result.EmployerEmail.Should().Be(resultFromService.ContactEmail);
        }
    }
}