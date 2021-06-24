using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingStartRegisterCourseDemandViewModelFromMediatorResult
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetStartCourseDemandQueryResult source)
        {
            //Act
            var actual = (StartRegisterCourseDemandViewModel) source;
            
            //Assert
            actual.TrainingCourse.Should().BeEquivalentTo(source.Course);
            actual.EntryPoint.Should().Be(source.EntryPoint);
        }
    }
}