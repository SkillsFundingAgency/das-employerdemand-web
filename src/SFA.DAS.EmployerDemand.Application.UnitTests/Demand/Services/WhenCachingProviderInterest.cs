using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenCachingProviderInterest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Created_In_The_Cache_For_Thirty_Minutes(
            IProviderDemandInterest item,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            DemandService service)
        {
            //Act
            await service.CreateCachedProviderInterest(item);
            
            //Assert
            cacheStorageService.Verify(x=>x.SaveToCache($"{item.Id}", item, TimeSpan.FromMinutes(30)));
        }
    }
}