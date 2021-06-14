using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromRegisterDemandRequestToRegisterCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(RegisterDemandRequest source)
        {
            var actual = (RegisterCourseDemandViewModel) source;
            
            actual.Should().BeEquivalentTo(source, options=>options
                .Excluding(c=>c.TrainingCourseId)
                .Excluding(c=>c.ExpiredCourseDemandId)
            );
            actual.ExpiredCourseDemandId.Should().BeNull();
        }
    }
}