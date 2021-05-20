using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenCreatingCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Cached_Item_Is_Returned_And_Service_Called_To_Create(
            CreateCourseDemandCommand command,
            [Frozen] Mock<IDemandService> service,
            CreateCourseDemandCommandHandler handler)
        {
            await handler.Handle(command, CancellationToken.None);
            
            service.Verify(x=>x.CreateCourseDemand(command.Id, command.ResponseUrl), Times.Once);
        }
    }
}