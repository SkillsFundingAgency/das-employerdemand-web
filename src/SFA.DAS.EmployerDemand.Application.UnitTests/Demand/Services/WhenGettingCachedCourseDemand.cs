using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenGettingCachedCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Retrieved_From_The_Cache(
            Guid employerDemandKey,
            ICourseDemand item,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            DemandService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<ICourseDemand>(employerDemandKey.ToString()))
                .ReturnsAsync(item);
            
            //Act
            var actual = await service.GetCachedCourseDemand(employerDemandKey);
            
            //Assert
            actual.Should().BeEquivalentTo(item);
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Doesnt_Exist_Null_Is_Returned(
            Guid employerDemandKey,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            DemandService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<ICourseDemand>(It.IsAny<string>()))
                .ReturnsAsync((ICourseDemand)null);
            
            //Act
            var actual = await service.GetCachedCourseDemand(employerDemandKey);
            
            //Assert
            actual.Should().BeNull();
        }
    }
}