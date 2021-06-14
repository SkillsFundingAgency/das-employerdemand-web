using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Services;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Services
{
    public class WhenStoppingEmployerCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called(
            Guid demandId,
            StopEmployerCourseDemandResponse apiResponse,
            [Frozen] Mock<IApiClient> mockApiClient,
            DemandService service)
        {
            mockApiClient
                .Setup(client => client.Post<StopEmployerCourseDemandResponse, object>(
                    It.Is<PostStopEmployerCourseDemandRequest>(request => request.PostUrl.Contains($"demand/{demandId}/stop"))))
                .ReturnsAsync(apiResponse);

            var actual = await service.StopEmployerCourseDemand(demandId);

            actual.Should().BeEquivalentTo((StoppedCourseDemand)apiResponse);
        }
    }
}