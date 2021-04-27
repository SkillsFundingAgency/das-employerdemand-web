using System.Collections.Generic;
using System.Linq;
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
            actual.GetUrl.Should().Be($"demand/aggregated/providers/{ukprn}?courseId=&location=&locationRadius=&routes=");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_Course_Filter(int ukprn, int courseId)
        {
            //Act
            var actual = new GetProviderEmployerDemandRequest(ukprn, courseId);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/aggregated/providers/{ukprn}?courseId={courseId}&location=&locationRadius=&routes=");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_Location_Filter(int ukprn, string location, string locationRadius)
        {
            //Arrange
            var locationParam = $"{location}, {location}";
            //Act
            var actual = new GetProviderEmployerDemandRequest(ukprn,null, locationParam, locationRadius);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/aggregated/providers/{ukprn}?courseId=&location={HttpUtility.UrlEncode(locationParam)}&locationRadius={locationRadius}&routes=");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_Routes(int ukprn, List<string> selectedRoutes)
        {
            //Act
            var actual = new GetProviderEmployerDemandRequest(ukprn,null, null, null, selectedRoutes);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/aggregated/providers/{ukprn}?courseId=&location=&locationRadius=&routes=" 
                                      + string.Join("&routes=", selectedRoutes.Select(HttpUtility.UrlEncode)));
        }

        [Test, AutoData]
        public void Then_If_There_Is_A_SelectedCourse_And_Routes_Only_SelectedCourse_Is_Added(int ukprn, List<string> selectedRoutes, int courseId)
        {
            //Act
            var actual = new GetProviderEmployerDemandRequest(ukprn,courseId, null, null, selectedRoutes);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/aggregated/providers/{ukprn}?courseId={courseId}&location=&locationRadius=&routes=");
        }
    }
}