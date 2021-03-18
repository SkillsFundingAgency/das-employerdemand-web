using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingThePostCreateDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_And_Data_Populated(PostCreateDemandData request)
        {
            //Act
            var actual = new PostCreateDemandRequest(request);
            
            //Assert
            actual.PostUrl.Should().Be("demand/create");
            actual.Data.Should().BeEquivalentTo(request);
        }
    }
}