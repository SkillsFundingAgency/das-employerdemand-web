using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenCreatingCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Cache_Is_Read_And_Api_Is_Called(
            Guid id,
            PostCreateDemandData demand,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {

            cacheStorageService
                .Setup(x => x.RetrieveFromCache<PostCreateDemandData>(id.ToString()))
                .ReturnsAsync(demand);
            
            await service.CreateCourseDemand(id);
            
            apiClient.Verify(x=>x.Post<Guid,PostCreateDemandData>(It.Is<PostCreateDemandRequest>(c=>
                c.Data.Id.Equals(demand.Id)
                && c.Data.ContactEmailAddress.Equals(demand.ContactEmailAddress)
                )));
        }
        
    }
}