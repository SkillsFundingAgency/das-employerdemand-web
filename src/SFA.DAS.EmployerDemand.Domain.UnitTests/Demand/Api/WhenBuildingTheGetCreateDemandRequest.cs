using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingTheGetCreateDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(int trainingCourseId)
        {
            //Act
            var actual = new GetCreateDemandRequest(trainingCourseId);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/create/{trainingCourseId}");
        }
    }
}