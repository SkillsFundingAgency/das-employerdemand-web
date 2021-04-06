using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenGettingProviderEmployerOpportunitiesForCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_With_The_Query(
            int ukprn,
            int courseId,
            string location,
            string radius,
            GetProviderEmployerDemandDetailsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediator.Setup(x =>
                x.Send(It.Is<GetProviderEmployerDemandDetailsQuery>(c =>
                    c.Ukprn.Equals(ukprn) 
                    && c.CourseId.Equals(courseId) 
                    && c.Location.Equals(location)
                    && c.LocationRadius.Equals(radius)
                    ), CancellationToken.None)).ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunitiesForCourse(ukprn, courseId, location, radius) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandDetailsViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Should().BeEquivalentTo((AggregatedProviderCourseDemandDetailsViewModel)mediatorResult);
        }
    }
}