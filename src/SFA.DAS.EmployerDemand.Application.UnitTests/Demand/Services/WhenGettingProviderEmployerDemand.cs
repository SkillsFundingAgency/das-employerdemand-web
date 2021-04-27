using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
    public class WhenGettingProviderEmployerDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Employer_Demand_For_A_Provider_With_Filter(
            int ukprn,
            int? courseId,
            string location,
            string locationRadius,
            List<string> selectedRoutes,
            Guid id,
            GetProviderEmployerDemandResponse response,
            [Frozen] Mock<IApiClient> apiClient,
            DemandService service)
        {
            //Arrange
            apiClient.Setup(x =>
                x.Get<GetProviderEmployerDemandResponse>(It.Is<GetProviderEmployerDemandRequest>(c =>
                    c.GetUrl.Contains($"/{ukprn}?courseId={courseId}&location={HttpUtility.UrlEncode(location)}&locationRadius={locationRadius}&routes="))))
                .ReturnsAsync(response);
            
            //Act
            var actual = await service.GetProviderEmployerDemand(ukprn, courseId, location, locationRadius, selectedRoutes);
            
            //Act
            actual.Should().BeEquivalentTo(response);
        }
    }
}