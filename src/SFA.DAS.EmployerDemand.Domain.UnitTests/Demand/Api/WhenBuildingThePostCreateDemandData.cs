using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Locations;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingThePostCreateDemandData
    {
        [Test, AutoData]
        public void Then_The_Location_Is_Mapped(Location source)
        {
            var actual = new PostCreateDemandData
            {
                LocationItem = source
            };
            
            actual.CourseDemandLocation.Name.Should().BeEquivalentTo(source.Name);
            actual.CourseDemandLocation.LocationPoint.GeoPoint.Should().BeEquivalentTo(source.LocationPoint);
        }
        
        [Test, AutoData]
        public void Then_The_TrainingCourse_Is_Mapped(Course source)
        {
            var actual = new PostCreateDemandData
            {
                Course = source
            };
            
            actual.TrainingCourse.Should().BeEquivalentTo(source);
        }
    }
}