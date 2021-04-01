using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingTheGetProviderEmployerDemand
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(int ukprn)
        {
            //Act
            var actual = new GetProviderEmployerDemandRequest(ukprn);
            
            //Assert
            actual.GetUrl.Should().Be($"providers/{ukprn}/employer-demand?courseId=&location=&locationRadius=");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_Course_Filter(int ukprn, int courseId)
        {
            //Act
            var actual = new GetProviderEmployerDemandRequest(ukprn, courseId);
            
            //Assert
            actual.GetUrl.Should().Be($"providers/{ukprn}/employer-demand?courseId={courseId}&location=&locationRadius=");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_Location_Filter(int ukprn, string location, string locationRadius)
        {
            //Arrange
            var locationParam = $"{location}, {location}";
            //Act
            var actual = new GetProviderEmployerDemandRequest(ukprn,null, locationParam, locationRadius);
            
            //Assert
            actual.GetUrl.Should().Be($"providers/{ukprn}/employer-demand?courseId=&location={HttpUtility.UrlEncode(locationParam)}&locationRadius={locationRadius}");
        }
    }
}