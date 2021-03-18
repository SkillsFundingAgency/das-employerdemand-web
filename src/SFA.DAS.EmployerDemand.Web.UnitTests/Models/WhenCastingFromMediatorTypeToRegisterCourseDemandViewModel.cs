using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromMediatorTypeToRegisterCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(CourseDemandRequest source)
        {
            //Act
            var actual = (RegisterCourseDemandViewModel) source;
            
            //Assert
            actual.Location.Should().Be(source.LocationItem.Name);
            actual.TrainingCourse.Should().BeEquivalentTo(source.Course);
            actual.Should().BeEquivalentTo(source, options=>options
                .Excluding(c=>c.LocationItem)
                .Excluding(c=>c.Location)
                .Excluding(c=>c.Id)
                .Excluding(c=>c.TrainingCourseId)
                .Excluding(c=>c.Course)
            );
        }
    }
}