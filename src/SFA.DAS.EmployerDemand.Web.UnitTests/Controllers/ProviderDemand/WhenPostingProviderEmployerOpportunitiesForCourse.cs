using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
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

            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("FindApprenticeshipTrainingOpportunitiesForCourse");
            var model = actual.Model as AggregatedProviderCourseDemandDetailsViewModel;
            model.SelectedEmployerDemandIds.Should().BeEquivalentTo(request.EmployerDemandIds);
            model.Should().BeEquivalentTo((AggregatedProviderCourseDemandDetailsViewModel)resultForGet, options => options.Excluding(viewModel => viewModel.SelectedEmployerDemandIds));
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Validation_Error_Then_The_Command_Is_Handled_And_Redirected(
            ProviderRegisterInterestRequest request,
            CreateCachedProviderInterestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
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
                            && c.Course.Sector.Equals(request.CourseSector)
                            && c.EmployerDemandIds.Count().Equals(request.EmployerDemandIds.Count)),
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
        public async Task Then_If_There_Is_No_Validation_Error_Then_The_Command_Is_Handled_And_Redirected_To_Edit_If_No_Email(
            ProviderRegisterInterestRequest request,
            CreateCachedProviderInterestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            request.ProviderEmail = string.Empty;
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
            ProviderRegisterInterestRequest request,
            CreateCachedProviderInterestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            request.ProviderTelephoneNumber = string.Empty;
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
    }
}