using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.UpdateCachedProviderInterest;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedProviderInterest;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenPostingEditProviderInterest
    {
        [Test, MoqAutoData]
        public async Task Then_If_Validation_Exception_The_View_Is_Returned_With_Errors(
            GetCachedProviderInterestQueryResult queryResult,
            UpdateProviderInterestDetails request,
            UpdateCachedProviderInterestCommandResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediator.Setup(x =>
                x.Send(It.Is<GetCachedProviderInterestQuery>(c => c.Id.Equals(request.Id)),
                    It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);
            mediator.Setup(x => x.Send(
                It.Is<UpdateCachedProviderInterestCommand>(c =>
                    c.Id.Equals(request.Id)
                    && c.Website.Equals(request.Website)
                    && c.PhoneNumber.Equals(request.PhoneNumber)
                    && c.EmailAddress.Equals(request.EmailAddress)
                ),
                It.IsAny<CancellationToken>())).ThrowsAsync(new ValidationException());
            
            //Act
            var actual = await controller.PostEditProviderDetails(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("EditProviderDetails");
            var actualModel = actual.Model as ProviderContactDetailsViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Should().BeEquivalentTo(queryResult.ProviderInterest, options=>options
                .Excluding(c=>c.Website)
                .Excluding(c=>c.PhoneNumber)
                .Excluding(c=>c.EmailAddress)
                .Excluding(c=>c.EmployerDemands)
                .Excluding(c => c.ProviderOffersThisCourse)
            );
            actualModel.Website.Should().Be(request.Website);
            actualModel.PhoneNumber.Should().Be(request.PhoneNumber);
            actualModel.EmailAddress.Should().Be(request.EmailAddress);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Valid_Redirects_To_Confirm_Details(
            UpdateProviderInterestDetails request,
            UpdateCachedProviderInterestCommandResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Assert
            mediator.Setup(x => x.Send(
                It.Is<UpdateCachedProviderInterestCommand>(c => 
                    c.Id.Equals(request.Id)
                    && c.Website.Equals(request.Website)
                    && c.PhoneNumber.Equals(request.PhoneNumber)
                    && c.EmailAddress.Equals(request.EmailAddress)
                    ),
                It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostEditProviderDetails(request) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ConfirmProviderDetails);
            actual.RouteValues["id"].Should().Be(result.Id);
            actual.RouteValues["ukprn"].Should().Be(request.Ukprn);
            actual.RouteValues["courseId"].Should().Be(request.CourseId);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Valid_But_Null_Returned_Then_Redirected_To_ProviderDemandDetails(
            UpdateProviderInterestDetails request,
            UpdateCachedProviderInterestCommandResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            result.Id = null;
            mediator.Setup(x => x.Send(
                It.Is<UpdateCachedProviderInterestCommand>(c => 
                    c.Id.Equals(request.Id)
                    && c.Website.Equals(request.Website)
                    && c.PhoneNumber.Equals(request.PhoneNumber)
                    && c.EmailAddress.Equals(request.EmailAddress)
                ),
                It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostEditProviderDetails(request) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ProviderDemandDetails);
            actual.RouteValues["ukprn"].Should().Be(request.Ukprn);
            actual.RouteValues["courseId"].Should().Be(request.CourseId);
        }
    }
}