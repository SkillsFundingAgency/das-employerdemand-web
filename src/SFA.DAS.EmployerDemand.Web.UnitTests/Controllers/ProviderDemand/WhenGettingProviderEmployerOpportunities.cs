using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
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
            mediatorResult.SelectedRadius = null;
            mediatorResult.SelectedLocation = null;
            mediatorResult.SelectedCourseId = null;
            mediatorResult.SelectedSectors = new List<Sector>();

            config.Object.Value.ProviderPortalUrl = portalUrl;

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            Assert.IsNotNull(actualModel);
            actualModel.Courses.Should().BeEquivalentTo(mediatorResult.Courses);
            actualModel.CourseDemands.Should().BeEquivalentTo(mediatorResult.CourseDemands);
            actualModel.TotalResults.Should().Be(mediatorResult.TotalResults);
            actualModel.TotalFiltered.Should().Be(mediatorResult.TotalFiltered);
            actualModel.SelectedCourse.Should().BeNullOrEmpty();
            actualModel.ShowFilterOptions.Should().BeFalse();
            actualModel.SelectedRadius.Should().Be("5");
            actualModel.SelectedSectors.Should().BeEmpty();
            actual.ViewData["ProviderDashboard"].Should().Be(portalUrl);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Course_And_Location_Selected_Then_The_SelectedField_Is_Populated_On_The_Model(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = mediatorResult.Courses.First().Id;

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.Courses.Should().BeEquivalentTo(mediatorResult.Courses);
            actualModel.TotalResults.Should().Be(mediatorResult.TotalResults);
            actualModel.TotalFiltered.Should().Be(mediatorResult.TotalFiltered);
            actualModel.CourseDemands.Should().BeEquivalentTo(mediatorResult.CourseDemands);
            var selectedCourse = mediatorResult.Courses.Single(c => c.Id.Equals(mediatorResult.SelectedCourseId));
            actualModel.SelectedCourse.Should().Be($"{selectedCourse.Title} (level {selectedCourse.Level})");
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.Location.Should().Be(mediatorResult.SelectedLocation.Name);
        }

        [Test, MoqAutoData]
        public async Task Then_Show_Filter_Options_On_The_Model_Is_True_If_Just_Location(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = null;
            mediatorResult.SelectedSectors = new List<Sector>();

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_Show_Filter_Options_On_The_Model_Is_True_If_Just_Course(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = mediatorResult.Courses.First().Id;
            mediatorResult.SelectedLocation = null;
            mediatorResult.SelectedSectors = new List<Sector>();

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_Show_Filter_Options_On_The_Model_Is_True_If_Just_Sector(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = null;
            mediatorResult.SelectedLocation = null;
            mediatorResult.SelectedSectors = new List<Sector> { new Sector { Id = new Guid(), Route = "route" } };

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Location_Link_Is_Built_If_Course_Selected(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = mediatorResult.Courses.First().Id;
            mediatorResult.SelectedSectors = new List<Sector>();

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearLocationLink.Should().Be($"?selectedCourseId={mediatorResult.SelectedCourseId}");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Location_Link_Does_Not_Contain_Sectors_If_Course_Selected(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = mediatorResult.Courses.First().Id;
            mediatorResult.SelectedSectors = new List<Sector> { new Sector { Id = new Guid(), Route = "route" } };

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearLocationLink.Should().Be($"?selectedCourseId={mediatorResult.SelectedCourseId}");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Location_Link_Is_Built_If_Course_Not_Selected_And_Sector_Selected(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = null;
            mediatorResult.SelectedSectors = new List<Sector> { new Sector { Id = new Guid(), Route = "route" } };

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearLocationLink.Should()
                .Be($"?sectors={mediatorResult.SelectedSectors.FirstOrDefault().Route}");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Location_Link_Is_Built_If_Course_Not_Selected_And_Sector_Not_Selected(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = null;
            mediatorResult.SelectedSectors = new List<Sector>();

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearLocationLink.Should().Be("");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Course_Link_Is_Built(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = mediatorResult.Courses.First().Id;

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearCourseLink.Should().Be($"?location={HttpUtility.UrlEncode(actualModel.Location)}&radius={actualModel.SelectedRadius}");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Sector_Link_Is_Built_if_Course_Not_Selected_And_Location_Not_Selected(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = null;
            mediatorResult.SelectedLocation = null;
            mediatorResult.SelectedSectors = new List<Sector> {new Sector {Id = new Guid(), Route = "route"}};

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearSectorLink.Should().Contain(mediatorResult.SelectedSectors.FirstOrDefault().Route, $"?sectors={mediatorResult.SelectedSectors.FirstOrDefault().Route}");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Sector_Link_Is_Built_if_Course_Selected_And_Location_Not_Selected(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = mediatorResult.Courses.First().Id;
            mediatorResult.SelectedLocation = null;

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearSectorLink.Should().Contain(mediatorResult.SelectedSectors.FirstOrDefault().Route, $"?selectedCourseId={mediatorResult.SelectedCourseId}");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Sector_Link_Is_Built_if_Course_Not_Selected_And_Location_Selected(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = null;

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearSectorLink.Should().Contain(mediatorResult.SelectedSectors.FirstOrDefault().Route, $"?location={HttpUtility.UrlEncode(actualModel.Location)}&radius={actualModel.SelectedRadius}");
        }

        [Test, MoqAutoData]
        public async Task Then_The_Clear_Sector_Link_Is_Built_if_Course_Selected_And_Location_Selected(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = mediatorResult.Courses.First().Id;

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.ShowFilterOptions.Should().BeTrue();
            actualModel.ClearSectorLink.Should().Contain(mediatorResult.SelectedSectors.FirstOrDefault().Route, $"?selectedCourseId={mediatorResult.SelectedCourseId}&location={HttpUtility.UrlEncode(actualModel.Location)}&radius={actualModel.SelectedRadius}");
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Selected_Radius_Is_Invalid_It_Defaults_To_First_In_Dictionary(
            FindApprenticeshipTrainingOpportunitiesRequest request,
            GetProviderEmployerDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediatorResult.SelectedCourseId = null;
            mediatorResult.SelectedRadius = "s";

            SetupResponse(mediator, request, mediatorResult);

            //Act
            var actual = await controller.FindApprenticeshipTrainingOpportunities(request) as ViewResult;

            //Assert
            Assert.IsNotNull(actual);
            var actualModel = actual.Model as AggregatedProviderCourseDemandViewModel;
            actualModel.SelectedRadius.Should().Be("5");
        }

        private void SetupResponse(Mock<IMediator> mediator,
                FindApprenticeshipTrainingOpportunitiesRequest request, GetProviderEmployerDemandQueryResult result)
            {
                mediator.Setup(x =>
                    x.Send(It.Is<GetProviderEmployerDemandQuery>(c =>
                        c.Ukprn.Equals(request.Ukprn)
                        && c.CourseId.Equals(request.SelectedCourseId)
                        && c.Location.Equals(request.Location)
                        && c.LocationRadius.Equals(request.Radius)
                    ), CancellationToken.None)).ReturnsAsync(result);
            }
        }
}