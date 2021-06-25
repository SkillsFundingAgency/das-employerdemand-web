using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromProviderCourseDemandToProviderCourseDemandViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ProviderCourseDemand source)
        {
            //Act
            var actual = (ProviderCourseDemandViewModel) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
        }

        [Test]
        [InlineAutoData(0, "Unknown")]
        [InlineAutoData(1, "1 apprentice")]
        [InlineAutoData(4, "4 apprentices")]
        public void Then_The_Total_Apprentices_Text_Is_Correctly_Built(int totalApprentices, string expectedText, ProviderCourseDemand source)
        {
            //Arrange
            source.NumberOfApprentices = totalApprentices;
            
            //Act
            var actual = (ProviderCourseDemandViewModel) source;
            
            //Assert
            actual.NumberOfApprenticesTotalMessage.Should().Be(expectedText);
        }
    }
}