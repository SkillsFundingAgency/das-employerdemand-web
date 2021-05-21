using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromProviderInterestRequestToProviderContactDetailsViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ProviderInterestRequest source)
        {
            //Act
            var actual = (ProviderContactDetailsViewModel) source;
            
            //Assert
            actual.Course.Should().BeEquivalentTo(source.Course);
            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(c=>c.Course)
                .Excluding(c=>c.EmployerDemands)
                .Excluding(c=>c.ProviderName)
                .Excluding(c=>c.ProviderOffersThisCourse)
            );
            actual.RouteDictionary.Count.Should().Be(3);
            actual.RouteDictionary["ukprn"].Should().Be(source.Ukprn.ToString());
            actual.RouteDictionary["id"].Should().Be(source.Id.ToString());
            actual.RouteDictionary["courseId"].Should().Be(source.Course.Id.ToString());
        }
    }
}