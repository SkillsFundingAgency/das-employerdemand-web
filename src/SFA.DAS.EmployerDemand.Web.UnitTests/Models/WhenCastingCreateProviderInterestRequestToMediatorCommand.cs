using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateProviderInterest;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingCreateProviderInterestRequestToMediatorCommand
    {
        [Test, AutoData]
        public void Then_Maps_Fields(CreateProviderInterestRequest source)
        {
            var result = (CreateProviderInterestCommand) source;

            result.Id.Should().Be(source.Id);
        }
    }
}