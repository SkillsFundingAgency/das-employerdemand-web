using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromProviderCourseDemandDetailsToProviderCourseDemandDetailsViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ProviderCourseDemandDetails source)
        {
            //Act
            var actual = (ProviderCourseDemandDetailsViewModel) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
        }
    }
}