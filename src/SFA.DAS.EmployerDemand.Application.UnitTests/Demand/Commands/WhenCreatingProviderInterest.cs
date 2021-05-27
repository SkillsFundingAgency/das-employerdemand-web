using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateProviderInterest;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenCreatingProviderInterest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Calls_Service(
            CreateProviderInterestCommand command,
            [Frozen] Mock<IDemandService> service,
            CreateProviderInterestCommandHandler handler)
        {
            await handler.Handle(command, CancellationToken.None);
            
            service.Verify(x=>x.CreateProviderInterest(command.Id), Times.Once);
        }
    }
}