using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenVerifyingEmployerCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Data_Returned(
            Guid id,
            VerifyEmployerCourseDemandResponse response,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiClient.Setup(x =>
                x.Post<VerifyEmployerCourseDemandResponse>(
                    It.Is<PostVerifyEmployerCourseDemandRequest>(c => c.PostUrl.Contains($"demand/{id}/verify"))))
                .ReturnsAsync(response);
            
            //Act
            var actual = await service.VerifyEmployerCourseDemand(id);
            
            //Assert
            actual.Should().BeEquivalentTo(response, options=> options.Excluding(x=>x.Location));
        }
    }
}