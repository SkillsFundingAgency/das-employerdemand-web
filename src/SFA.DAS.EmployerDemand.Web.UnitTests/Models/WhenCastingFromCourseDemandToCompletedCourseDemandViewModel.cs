using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromCourseDemandToCompletedCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(VerifiedCourseDemand source)
        {
            //Act
            var actual = (CompletedCourseDemandViewModel) source;
            
            //Assert
            actual.Id.Should().Be(source.Id);
            actual.TrainingCourse.Should().BeEquivalentTo(source.Course);
            actual.LocationName.Should().Be(source.Location.Name);
            actual.ContactEmailAddress.Should().Be(source.ContactEmail);
            actual.NumberOfApprentices.Should().Be(source.NumberOfApprentices.ToString());
            
        }

        [Test, AutoData]
        public void Then_If_Apprentices_Not_Known_Then_Value_Set_To_Empty_String(VerifiedCourseDemand source)
        {
            //Arrange
            source.NumberOfApprentices = 0;
            
            //Act
            var actual = (CompletedCourseDemandViewModel) source;
            
            //Assert
            actual.NumberOfApprentices.Should().BeEmpty();
        }

        [Test]
        public void Then_If_The_Source_Is_Null_Then_Null_Returned()
        {
            //Act
            var actual = (CompletedCourseDemandViewModel) (VerifiedCourseDemand) null;
            
            //Assert
            actual.Should().BeNull();           
        }
        
        
        [Test, AutoData]
        public void Then_If_It_Is_A_Full_Postcode_Only_Postcode_And_Not_DistrictName_Is_Shown(VerifiedCourseDemand source, string district)
        {
            //Arrange
            var postcode = "CV1 1QT"; 
            source.Location.Name = $"{postcode}, {district}";
            //Act
            var actual = (CompletedCourseDemandViewModel) source;
            
            //Assert
            actual.LocationName.Should().Be(postcode);
        }
    }
}