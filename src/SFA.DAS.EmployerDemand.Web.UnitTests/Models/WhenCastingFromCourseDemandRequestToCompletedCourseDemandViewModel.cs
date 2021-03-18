using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromCourseDemandRequestToCompletedCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(CourseDemandRequest source)
        {
            //Act
            var actual = (CompletedCourseDemandViewModel) source;
            
            //Assert
            actual.TrainingCourse.Should().BeEquivalentTo(source.Course);
            actual.LocationName.Should().Be(source.LocationItem.Name);
            actual.ContactEmailAddress.Should().Be(source.ContactEmailAddress);
        }

        [Test]
        public void Then_If_The_Source_Is_Null_Then_Null_Returned()
        {
            //Act
            var actual = (CompletedCourseDemandViewModel) (CourseDemandRequest) null;
            
            //Assert
            actual.Should().BeNull();           
        }
    }
}