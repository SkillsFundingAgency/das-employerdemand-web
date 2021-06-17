using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingGetRestartCourseDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid id)
        {
            //Act
            var actual = new GetRestartCourseDemandRequest(id);
            
            //Assert
            actual.GetUrl.Should().Be($"demand/{id}/restart");
        }
    }
}