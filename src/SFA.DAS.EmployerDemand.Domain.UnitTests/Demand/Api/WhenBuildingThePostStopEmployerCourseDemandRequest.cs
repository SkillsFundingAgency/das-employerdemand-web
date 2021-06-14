using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingThePostStopEmployerCourseDemandRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Url_Is_Correct(
            Guid id)
        {
            var request = new PostStopEmployerCourseDemandRequest(id);

            request.PostUrl.Should().Be($"demand/{id}/stop");
        }
    }
}