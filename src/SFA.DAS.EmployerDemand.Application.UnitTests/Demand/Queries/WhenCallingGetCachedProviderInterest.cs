using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedProviderInterest;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Queries
{
    public class WhenCallingGetCachedProviderInterest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Retrieved_From_The_Service(
            GetCachedProviderInterestQuery query,
            ProviderInterestRequest result,
            [Frozen] Mock<IDemandService> service,
            GetCachedProviderInterestQueryHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetCachedProviderInterest(query.Id)).ReturnsAsync(result);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.ProviderInterest.Should().BeEquivalentTo(result);
        }
    }
}