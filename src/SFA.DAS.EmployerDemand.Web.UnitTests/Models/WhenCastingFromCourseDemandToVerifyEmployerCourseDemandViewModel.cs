using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromCourseDemandToVerifyEmployerCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(CourseDemand source)
        {
            //Act
            var actual = (VerifyEmployerCourseDemandViewModel) source;
            
            //Assert
            actual.Verified.Should().Be(source.EmailVerified);
            actual.EmailAddress.Should().Be(source.ContactEmailAddress);
        }

        [Test]
        public void Then_If_Source_Is_Null_Then_Null_Returned()
        {
            //Act
            var actual = (VerifyEmployerCourseDemandViewModel) (CourseDemand) null;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}