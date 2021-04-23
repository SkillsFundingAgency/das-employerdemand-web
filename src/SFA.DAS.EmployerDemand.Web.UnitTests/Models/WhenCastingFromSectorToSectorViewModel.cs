using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenCastingFromSectorToSectorViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(Sector source)
        {
            //Act
            var actual = new SectorViewModel(source, new List<string>());
            
            //Assert
            actual.Should().BeEquivalentTo(source);
            actual.Selected.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_Marked_As_Selected_If_In_List(Sector source)
        {
            //Act
            var actual = new SectorViewModel(source, new List<string>{source.Route});
            
            actual.Should().BeEquivalentTo(source);
            actual.Selected.Should().BeTrue();
        }
    }
}