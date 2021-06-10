using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingGetStartCourseDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(int courseId)
        {
            //Act
            var actual = new GetStartCourseDemandRequest(courseId);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/start/{courseId}");
        }
    }
}