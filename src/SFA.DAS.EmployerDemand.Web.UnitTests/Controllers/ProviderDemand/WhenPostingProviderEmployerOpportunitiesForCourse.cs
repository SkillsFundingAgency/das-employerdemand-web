using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenPostingProviderEmployerOpportunitiesForCourse
    {
        [Test, MoqAutoData]
        public async Task And_There_Is_A_Validation_Exception_Then_Register_View_Is_Returned(
            ProviderRegisterInterestRequest request,
            GetProviderEmployerDemandDetailsQueryResult resultForGet,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            ArrangeControllerContext(controller);
            request.EmployerDemands = new List<string>();
            mockMediator
                .Setup(x => x.Send(
                    It.IsAny<CreateCachedProviderInterestCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException());
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetProviderEmployerDemandDetailsQuery>(c =>
                        c.Ukprn.Equals(request.Ukprn) 
                        && c.CourseId.Equals(request.CourseId) 
                        && c.Location.Equals(request.Location)
                        && c.LocationRadius.Equals(request.Radius)), 
                    CancellationToken.None))
                .ReturnsAsync(resultForGet);

            var employerDemandIds = resultForGet.EmployerDemandIds.ToList().Take(3);

            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("FindApprenticeshipTrainingOpportunitiesForCourse");
            var model = actual.Model as AggregatedProviderCourseDemandDetailsViewModel;
            model.SelectedEmployerDemandIds.Should().BeEquivalentTo(employerDemandIds);
            model.Should().BeEquivalentTo((AggregatedProviderCourseDemandDetailsViewModel)resultForGet, options => options.Excluding(viewModel => viewModel.SelectedEmployerDemandIds));
        }

        [Test, MoqAutoData]
        public async Task And_No_Selected_Demands_Sends_Empty_List(
            Guid demandId,
            ProviderRegisterInterestRequest request,
            CreateCachedProviderInterestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            ArrangeControllerContext(controller);
            request.Id = null;
            request.EmployerDemands = null;

            mockMediator
                .Setup(x => x.Send(
                    It.Is<CreateCachedProviderInterestCommand>(c =>
                        c.Id != Guid.Empty
                        && c.Ukprn.Equals(request.Ukprn)
                        && c.Website.Equals(request.ProviderWebsite)
                        && c.EmailAddress.Equals(request.ProviderEmail)
                        && c.PhoneNumber.Equals(request.ProviderTelephoneNumber)
                        && c.Course.Id.Equals(request.CourseId)
                        && c.Course.Level.Equals(request.CourseLevel)
                        && c.Course.Title.Equals(request.CourseTitle)
                        && c.Course.Route.Equals(request.CourseSector)
                        && !c.EmployerDemands.Any()),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ConfirmProviderDetails);
            actual.RouteValues["id"].Should().Be(result.Id);
            actual.RouteValues["ukprn"].Should().Be(request.Ukprn);
            actual.RouteValues["courseId"].Should().Be(request.CourseId);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Validation_Error_Then_The_Command_Is_Handled_And_Redirected(
            Guid demandId,
            int numberOfApprentices,
            string demandLocation,
            ProviderRegisterInterestRequest request,
            CreateCachedProviderInterestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            ArrangeControllerContext(controller);
            request.Id = null;
            request.EmployerDemands = new List<string>
            {
                $"{demandId}|{numberOfApprentices}|{demandLocation}",
                $"{demandId}|{numberOfApprentices}|{demandLocation}",
                $"{demandId}|{numberOfApprentices}|{demandLocation}"
            };

            mockMediator
                .Setup(x => x.Send(
                    It.Is<CreateCachedProviderInterestCommand>(c =>
                            c.Id != Guid.Empty
                            && c.Ukprn.Equals(request.Ukprn)
                            && c.ProviderOffersThisCourse.Equals(request.ProviderOffersThisCourse)
                            && c.ProviderName.Equals(controller.User.Identity.Name)
                            && c.Website.Equals(request.ProviderWebsite)
                            && c.EmailAddress.Equals(request.ProviderEmail)
                            && c.PhoneNumber.Equals(request.ProviderTelephoneNumber)
                            && c.Course.Id.Equals(request.CourseId)
                            && c.Course.Level.Equals(request.CourseLevel)
                            && c.Course.Title.Equals(request.CourseTitle)
                            && c.Course.Route.Equals(request.CourseSector)
                            && c.EmployerDemands.ToList().TrueForAll(y=>y.LocationName.Equals(demandLocation))
                            && c.EmployerDemands.ToList().TrueForAll(y=>y.NumberOfApprentices.Equals(numberOfApprentices))
                            && c.EmployerDemands.ToList().TrueForAll(y=>y.EmployerDemandId.Equals(demandId))
                            ),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ConfirmProviderDetails);
            actual.RouteValues["id"].Should().Be(result.Id);
            actual.RouteValues["ukprn"].Should().Be(request.Ukprn);
            actual.RouteValues["courseId"].Should().Be(request.CourseId);
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Is_An_Update_The_Request_Id_Is_Passed_To_The_Handler(
            Guid demandId,
            ProviderRegisterInterestRequest request,
            CreateCachedProviderInterestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            ArrangeControllerContext(controller);
            request.EmployerDemands = new List<string>();
            mockMediator
                .Setup(x => x.Send(
                    It.Is<CreateCachedProviderInterestCommand>(c =>
                        c.Id.Equals(request.Id)),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ConfirmProviderDetails);
            actual.RouteValues["id"].Should().Be(result.Id);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Validation_Error_Then_The_Command_Is_Handled_And_Redirected_To_Edit_If_No_Email(
            Guid demandId,
            ProviderRegisterInterestRequest request,
            CreateCachedProviderInterestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            ArrangeControllerContext(controller);
            request.ProviderEmail = string.Empty;
            request.EmployerDemands = new List<string>
            {
                $"{demandId}|1|testLocation1",
                $"{demandId}|2|testLocation2",
                $"{demandId}|3|testLocation3"
            };

            mockMediator
                .Setup(x => x.Send(
                    It.Is<CreateCachedProviderInterestCommand>(c =>
                        c.Id != Guid.Empty
                        && c.Ukprn.Equals(request.Ukprn)),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.EditProviderDetails);
            actual.RouteValues["id"].Should().Be(result.Id);
            actual.RouteValues["ukprn"].Should().Be(request.Ukprn);
            actual.RouteValues["courseId"].Should().Be(request.CourseId);
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Validation_Error_Then_The_Command_Is_Handled_And_Redirected_To_Edit_If_No_PhoneNumber(
            Guid demandId,
            ProviderRegisterInterestRequest request,
            CreateCachedProviderInterestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            ArrangeControllerContext(controller);
            request.ProviderTelephoneNumber = string.Empty;
            request.EmployerDemands = new List<string>
            {
                $"{demandId}|1|testLocation1",
                $"{demandId}|2|testLocation2",
                $"{demandId}|3|testLocation3"
            };

            mockMediator
                .Setup(x => x.Send(
                    It.Is<CreateCachedProviderInterestCommand>(c =>
                        c.Id != Guid.Empty
                        && c.Ukprn.Equals(request.Ukprn)),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as RedirectToRouteResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.EditProviderDetails);
            actual.RouteValues["id"].Should().Be(result.Id);
            actual.RouteValues["ukprn"].Should().Be(request.Ukprn);
            actual.RouteValues["courseId"].Should().Be(request.CourseId);
        }

        private void ArrangeControllerContext(ControllerBase controller)
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext {User = principal}
            };
        }
    }
}