using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromMediatorTypeToConfirmCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetCachedCreateCourseDemandQueryResult source)
        {
            //Act
            var actual = (ConfirmCourseDemandViewModel) source;
            
            //Assert
            actual.TrainingCourse.Should().BeEquivalentTo(source.CourseDemand.Course);
            actual.LocationName.Should().Be(source.CourseDemand.LocationItem.Name);
            actual.NumberOfApprenticesKnown.Should().Be(source.CourseDemand.NumberOfApprenticesKnown ?? false);
            actual.NumberOfApprentices.Should().Be(source.CourseDemand.NumberOfApprentices);
            actual.OrganisationName.Should().Be(source.CourseDemand.OrganisationName);
            actual.ContactEmailAddress.Should().Be(source.CourseDemand.ContactEmailAddress);
        }
    }
}