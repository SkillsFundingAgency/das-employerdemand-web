using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingTheGetCreateDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_With_No_Location(int trainingCourseId)
        {
            //Act
            var actual = new GetCreateDemandRequest(trainingCourseId, "");
            
            //Assert
            actual.GetUrl.Should().Be($"demand/create/{trainingCourseId}?location=");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_And_Location_Encoded(int trainingCourseId, string location)
        {
            //Arrange
            var expectedLocation = $"{location}, {location} & {location}";
            
            //Act
            var actual = new GetCreateDemandRequest(trainingCourseId, expectedLocation);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/create/{trainingCourseId}?location={HttpUtility.UrlEncode(expectedLocation)}");
        }
    }
}