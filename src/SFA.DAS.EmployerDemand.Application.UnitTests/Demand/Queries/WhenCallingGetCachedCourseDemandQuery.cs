using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Queries
{
    public class WhenCallingGetCachedCourseDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Retrieved_From_The_Service(
            GetCachedCreateCourseDemandQuery query,
            CourseDemandRequest result,
            [Frozen] Mock<IDemandService> service,
            GetCachedCreateCourseDemandQueryHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetCachedCourseDemand(query.Id)).ReturnsAsync(result);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.CourseDemand.Should().BeEquivalentTo(result);
        }

    }
}