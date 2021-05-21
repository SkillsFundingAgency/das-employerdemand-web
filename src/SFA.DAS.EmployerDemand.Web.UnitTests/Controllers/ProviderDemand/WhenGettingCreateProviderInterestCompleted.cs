using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedProviderInterest;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenGettingCreateProviderInterestCompleted
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Model_Returned(
            Guid id,
            int ukprn,
            int courseId,
            GetCachedProviderInterestQueryResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            [Greedy] HomeController controller)
        {
            //Arrange
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetCachedProviderInterestQuery>(c =>
                        c.Id == id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);

            //Act
            var actual = await controller.CreateProviderInterestCompleted(ukprn, courseId, id) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as CreateProviderInterestCompletedViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Should().BeEquivalentTo((CreateProviderInterestCompletedViewModel)result.ProviderInterest, options => options
                .ExcludingMissingMembers()
                .Excluding(model => model.FindApprenticeshipTrainingUrl));
            actualModel.FindApprenticeshipTrainingUrl.Should()
                .Be(mockOptions.Object.Value.FindApprenticeshipTrainingUrl);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Returned_Redirected_To_ProviderDemandDetails(
            Guid id,
            int ukprn,
            int courseId,
            GetCachedProviderInterestQueryResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            result.ProviderInterest = null;
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetCachedProviderInterestQuery>(c =>
                        c.Id == id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);

            //Act
            var actual = await controller.CreateProviderInterestCompleted(ukprn, courseId, id) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ProviderDemandDetails);
            actual.RouteValues["ukprn"].Should().Be(ukprn);
            actual.RouteValues["courseId"].Should().Be(courseId);
        }
    }
}