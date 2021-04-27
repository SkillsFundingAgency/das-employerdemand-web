using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenGettingCachedProviderInterest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Retrieved_From_The_Cache(
            Guid providerInterestKey,
            ProviderInterestRequest item,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            DemandService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<ProviderInterestRequest>(providerInterestKey.ToString()))
                .ReturnsAsync(item);
            
            //Act
            var actual = await service.GetCachedProviderInterest(providerInterestKey);
            
            //Assert
            actual.Should().BeEquivalentTo(item);
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Doesnt_Exist_Null_Is_Returned(
            Guid providerInterestKey,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            DemandService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<IProviderDemandInterest>(It.IsAny<string>()))
                .ReturnsAsync((IProviderDemandInterest)null);
            
            //Act
            var actual = await service.GetCachedProviderInterest(providerInterestKey);
            
            //Assert
            actual.Should().BeNull();
        }
    }
}