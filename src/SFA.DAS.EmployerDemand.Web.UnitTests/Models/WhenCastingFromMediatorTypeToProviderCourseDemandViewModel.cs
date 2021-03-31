using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromMediatorTypeToProviderCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProviderEmployerDemandQueryResult source)
        {
            //Act
            var actual = (AggregatedProviderCourseDemandViewModel) source;

            //Assert
            actual.Courses.Should().BeEquivalentTo(source.Courses);
            actual.TotalResults.Should().Be(source.TotalResults);
            actual.TotalFiltered.Should().Be(source.TotalFiltered);
            actual.CourseDemands.Should().BeEquivalentTo(source.CourseDemands);
        }
    }
}