using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingPostVerifyEmployerCourseDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(Guid id)
        {
            //Act
            var actual = new PostVerifyEmployerCourseDemandRequest(id);
            
            //Assert
            actual.PostUrl.Should().Be($"demand/{id}/verify");
        }
    }
}