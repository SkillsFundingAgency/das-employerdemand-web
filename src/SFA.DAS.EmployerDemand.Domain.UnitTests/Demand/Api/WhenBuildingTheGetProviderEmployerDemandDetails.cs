using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingTheGetProviderEmployerDemandDetails
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(int ukprn, int courseId)
        {
            //Act
            var actual = new GetProviderEmployerDemandDetailsRequest(ukprn, courseId);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/aggregated/providers/{ukprn}/courses/{courseId}?location=&locationRadius=");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_Location_Filter(int ukprn, int courseId, string location, string locationRadius)
        {
            //Arrange
            var locationParam = $"{location}, {location}";
            //Act
            var actual = new GetProviderEmployerDemandDetailsRequest(ukprn, courseId, locationParam, locationRadius);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/aggregated/providers/{ukprn}/courses/{courseId}?location={HttpUtility.UrlEncode(locationParam)}&locationRadius={locationRadius}");
        }
    }
}