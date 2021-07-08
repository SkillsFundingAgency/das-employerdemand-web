using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models.AggregatedProviderCourseDemandDetailsViewModelTests
{
    public class WhenCastingFromMediatorTypeToAggregatedProviderCourseDemandDetailsViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Arrange
            source.SelectedRadius = null;
            source.SelectedLocation = null;
            
            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;

            //Assert
            actual.Course.Should().BeEquivalentTo(source.Course);
            actual.CourseDemandDetailsList.Should().BeEquivalentTo(source.CourseDemandDetailsList.Select(details => (ProviderCourseDemandDetailsViewModel)details));
            actual.ShowFilterOptions.Should().BeFalse();
            actual.SelectedRadius.Should().Be("5");
            actual.SelectedEmployerDemandIds.Should().BeEquivalentTo(source.EmployerDemandIds);
        }

        [Test, AutoData]
        public void Then_If_There_Is_A_Location_Selected_Then_The_SelectedField_Is_Populated(GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Arrange
            source.SelectedRadius = "10";

            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;

            //Assert
            actual.Course.Should().BeEquivalentTo(source.Course);
            actual.CourseDemandDetailsList.Should().BeEquivalentTo(source.CourseDemandDetailsList.Select(details => (ProviderCourseDemandDetailsViewModel)details));
            actual.SelectedRadius.Should().Be(source.SelectedRadius);
            actual.ShowFilterOptions.Should().BeTrue();
            actual.Location.Should().Be(source.SelectedLocation.Name);
        }

        [Test, AutoData]
        public void Then_Show_Filter_Options_Is_True_If_Just_Location(GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;
            
            //Assert
            actual.ShowFilterOptions.Should().BeTrue();
        }

        [Test, AutoData]
        public void Then_If_The_Selected_Radius_Is_Invalid_It_Defaults_To_First_In_Dictionary(GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Arrange
            source.SelectedRadius = "s";
            
            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;

            //Assert
            actual.SelectedRadius.Should().Be("5");
        }

        [Test, AutoData]
        public void Then_The_Provider_Details_Are_Added_To_The_Model(GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;
            
            //Assert
            actual.ProviderOffersThisCourse.Should().BeTrue();
            actual.ProviderEmail.Should().Be(source.ProviderContactDetails.EmailAddress);
            actual.ProviderTelephoneNumber.Should().Be(source.ProviderContactDetails.PhoneNumber);
            actual.ProviderWebsite.Should().Be(source.ProviderContactDetails.Website);
        }

        [Test, AutoData]
        public void Then_If_The_ProviderContactDetails_Are_Null_Then_Empty_Is_Set(GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Arrange
            source.ProviderContactDetails = null;
            
            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;
            
            //Assert
            actual.ProviderOffersThisCourse.Should().BeFalse();
            actual.ProviderEmail.Should().BeEmpty();
            actual.ProviderTelephoneNumber.Should().BeEmpty();
            actual.ProviderWebsite.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_If_The_EmployerDemandIds_Are_Null_Then_Empty_Is_Set(
            GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Arrange
            source.EmployerDemandIds = null;

            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;

            //Assert
            actual.SelectedEmployerDemandIds.Should().BeEquivalentTo(new List<Guid>());
        }

        [Test, AutoData]
        public void Then_If_Id_Is_Null_Then_Null_Set(
            GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Arrange
            source.Id = null;

            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;

            //Assert
            actual.Id.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_If_Id_Is_Empty_Guid_Then_Null_Set(
            GetProviderEmployerDemandDetailsQueryResult source)
        {
            //Arrange
            source.Id = Guid.Empty;

            //Act
            var actual = (AggregatedProviderCourseDemandDetailsViewModel) source;

            //Assert
            actual.Id.Should().BeNull();
        }
    }
}