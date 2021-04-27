using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromCourseDemandRequestToConfirmCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(CourseDemandRequest source)
        {
            //Act
            var actual = (ConfirmCourseDemandViewModel) source;
            
            //Assert
            actual.Id.Should().Be(source.Id);
            actual.TrainingCourse.Should().BeEquivalentTo(source.Course);
            actual.LocationName.Should().Be(source.LocationItem.Name);
            actual.NumberOfApprenticesKnown.Should().Be(source.NumberOfApprenticesKnown ?? false);
            actual.NumberOfApprentices.Should().Be(source.NumberOfApprentices);
            actual.OrganisationName.Should().Be(source.OrganisationName);
            actual.ContactEmailAddress.Should().Be(source.ContactEmailAddress);
        }

        [Test, AutoData]
        public void Then_Null_Is_Returned_If_The_Source_Is_Null()
        {   
            //Act
            var actual = (ConfirmCourseDemandViewModel) (CourseDemandRequest) null;
            
            //Assert
            actual.Should().BeNull();
        }
        
        
        [Test, AutoData]
        public void Then_If_It_Is_A_Full_Postcode_Only_Postcode_And_Not_DistrictName_Is_Shown(CourseDemandRequest source, string district)
        {
            //Arrange
            var postcode = "CV1 1QT"; 
            source.LocationItem.Name = $"{postcode}, {district}";
            //Act
            var actual = (ConfirmCourseDemandViewModel) source;
            
            //Assert
            actual.LocationName.Should().Be(postcode);
        }
    }
}