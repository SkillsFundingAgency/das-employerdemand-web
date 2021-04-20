using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Components.RenderTree;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromMediatorTypeToAggregatedProviderCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedRadius = null;
            source.SelectedLocation = null;
            source.SelectedCourseId = null;
            source.Sectors = new List<Sector>();

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.Courses.Should().BeEquivalentTo(source.Courses);
            actual.TotalResults.Should().Be(source.TotalResults);
            actual.TotalFiltered.Should().Be(source.TotalFiltered);
            actual.CourseDemands.Should().BeEquivalentTo(source.CourseDemands);
            actual.SelectedCourse.Should().BeEmpty();
            actual.ShowFilterOptions.Should().BeFalse();
            actual.SelectedRadius.Should().Be("5");
            actual.SelectedSectors.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_If_There_Is_A_Course_And_Location_Selected_Then_The_SelectedField_Is_Populated(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = source.Courses.First().Id;
            
            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.Courses.Should().BeEquivalentTo(source.Courses);
            actual.TotalResults.Should().Be(source.TotalResults);
            actual.TotalFiltered.Should().Be(source.TotalFiltered);
            actual.CourseDemands.Should().BeEquivalentTo(source.CourseDemands);
            var selectedCourse = source.Courses.Single(c => c.Id.Equals(source.SelectedCourseId));
            actual.SelectedCourse.Should().Be($"{selectedCourse.Title} (level {selectedCourse.Level})");
            actual.ShowFilterOptions.Should().BeTrue();
            actual.Location.Should().Be(source.SelectedLocation.Name);
        }

        [Test, AutoData]
        public void Then_Show_Filter_Options_Is_True_If_Just_Location(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = null;
            source.Sectors = new List<Sector>();

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;
            
            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
        }
        
        [Test, AutoData]
        public void Then_Show_Filter_Options_Is_True_If_Just_Course(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = source.Courses.First().Id;
            source.SelectedLocation = null;
            source.Sectors = new List<Sector>();

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;
            
            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
        }

        [Test, AutoData]
        public void Then_Show_Filter_Options_Is_True_If_Just_Sector(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = null;
            source.SelectedLocation = null;
            source.Sectors = new List<Sector> {new Sector {Id = new Guid(), Route = "route"}};

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel)source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
        }

        [Test, AutoData]
        public void Then_The_Clear_Location_Link_Is_Built_If_Course_Selected(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = source.Courses.First().Id;
            source.Sectors = new List<Sector>();

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearLocationLink.Should().Be($"?selectedCourseId={source.SelectedCourseId}");
        }

        [Test, AutoData]
        public void Then_The_Clear_Location_Link_Does_Not_Contain_Sectors_If_Course_Selected(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = source.Courses.First().Id;
            source.Sectors = new List<Sector> {new Sector{Id = new Guid(), Route = "route"}};

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel)source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearLocationLink.Should().Be($"?selectedCourseId={source.SelectedCourseId}");
        }

        [Test, AutoData]
        public void Then_The_Clear_Location_Link_Is_Built_If_Sectors_Selected(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = null;
            source.Sectors = new List<Sector> { new Sector { Id = new Guid(), Route = "route" } };

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel)source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearLocationLink.Should().Be($"?sectors={source.Sectors.FirstOrDefault().Route}");
        }

        [Test, AutoData]
        public void Then_The_Clear_Location_Link_Is_Built_If_No_Course_And_Sector_Selected(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = null;
            source.Sectors = new List<Sector>();

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearLocationLink.Should().Be("");
        }
        
        [Test, AutoData]
        public void Then_The_Clear_Course_Link_Is_Built(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = source.Courses.First().Id;
            
            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearCourseLink.Should().Be($"?location={HttpUtility.UrlEncode(actual.Location)}&radius={actual.SelectedRadius}");
        }

        [Test, AutoData]
        public void Then_The_Clear_Sector_Link_Is_Built_If_No_Course_And_Location_Selected(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = null;
            source.SelectedLocation = null;

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearSectorLink.Should().Be("");
        }

        [Test, AutoData]
        public void Then_The_Clear_Sector_Link_Is_Built_If_Course_Selected_And_No_Location_Selected(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = source.Courses.First().Id;
            source.SelectedLocation = null;

            //Act
            var actual = (AggregatedProviderCourseDemandViewModel)source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearSectorLink.Should().Be($"?selectedCourseId={source.SelectedCourseId}");
        }

        [Test, AutoData]
        public void Then_The_Clear_Sector_Link_Is_Built_If_Location_Selected_And_No_Course_Selected(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = null;
            
            //Act
            var actual = (AggregatedProviderCourseDemandViewModel)source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearSectorLink.Should().Be($"?location={HttpUtility.UrlEncode(actual.Location)}&radius={actual.SelectedRadius}");
        }

        [Test, AutoData]
        public void Then_The_Clear_Sector_Link_Is_Built_If_Course_And_Location_Selected(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = source.Courses.First().Id;
            
            //Act
            var actual = (AggregatedProviderCourseDemandViewModel)source;

            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
            actual.ClearSectorLink.Should().Be($"?selectedCourseId={source.SelectedCourseId}&location={HttpUtility.UrlEncode(actual.Location)}&radius={actual.SelectedRadius}");
        }

        [Test, AutoData]
        public void Then_If_The_Selected_Radius_Is_Invalid_It_Defaults_To_First_In_Dictionary(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = null;
            source.SelectedRadius = "s";
            
            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.SelectedRadius.Should().Be("5");
        }
    }
}