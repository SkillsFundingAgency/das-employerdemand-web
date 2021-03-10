using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromMediatorTypeToTrainingCourseViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(TrainingCourse source)
        {
            //Act
            var actual = (TrainingCourseViewModel) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
            actual.TitleAndLevel.Should().Be($"{source.Title} (level {source.Level})");
        }

        [Test]
        public void Then_If_Null_Then_Null_Is_Returned()
        {
            //Act
            var actual = (TrainingCourseViewModel) (TrainingCourse)null;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}