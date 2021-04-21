using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedProviderInterest;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenGettingConfirmProviderInterest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Model_Returned(
            Guid id,
            int ukprn,
            int courseId,
            GetCachedProviderInterestQueryResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetCachedProviderInterestQuery>(c =>
                        c.Id == id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            //Act
            var actual = await controller.ConfirmProviderDetails(ukprn, courseId, id) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as ProviderContactDetailsViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Id.Should().Be(result.ProviderInterest.Id);
        }
    }
}