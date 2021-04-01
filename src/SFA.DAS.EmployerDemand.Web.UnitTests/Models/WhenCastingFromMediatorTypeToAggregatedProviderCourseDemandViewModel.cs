using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Components.RenderTree;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromMediatorTypeToAggregatedProviderCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedLocation = null;
            source.SelectedCourseId = null;
            
            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.Courses.Should().BeEquivalentTo(source.Courses);
            actual.TotalResults.Should().Be(source.TotalResults);
            actual.TotalFiltered.Should().Be(source.TotalFiltered);
            actual.CourseDemands.Should().BeEquivalentTo(source.CourseDemands);
            actual.SelectedCourse.Should().BeEmpty();
            actual.ShowFilterOptions.Should().BeFalse();
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
            actual.SelectedCourse.Should().Be(source.Courses.Single(c => c.Id.Equals(source.SelectedCourseId)).Title);
            actual.ShowFilterOptions.Should().BeTrue();
            actual.Location.Should().Be(source.SelectedLocation.Name);
        }

        [Test, AutoData]
        public void Then_Show_Filter_Options_Is_True_If_Just_Location(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = null;
            
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
            
            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;
            
            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
        }

        [Test, AutoData]
        public void Then_The_Clear_Location_Link_Is_Built(GetProviderEmployerDemandQueryResult source)
        {
            //Arrange
            source.SelectedCourseId = source.Courses.First().Id;
            
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
            actual.ClearCourseLink.Should().Be("");
        }
    }
}