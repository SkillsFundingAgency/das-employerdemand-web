using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenGettingProviderEmployerOpportunities
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_With_The_Query(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            string portalUrl,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> config,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            config.Object.Value.ProviderPortalUrl = portalUrl;
            mediator.Setup(x =>
                x.Send(It.Is<GetProviderEmployerDemandQuery>(c =>
                    c.Ukprn.Equals(request.Ukprn) 
                    && c.CourseId.Equals(request.SelectedCourseId) 
                    && c.Location.Equals(request.Location)
                    && c.LocationRadius.Equals(request.Radius)
                    ), CancellationToken.None)).ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Courses.Should().BeEquivalentTo(mediatorResult.Courses);
            actualModel.CourseDemands.Should().BeEquivalentTo(mediatorResult.CourseDemands);
            actual.ViewData["ProviderDashboard"].Should().Be(portalUrl);
        }
    }
}