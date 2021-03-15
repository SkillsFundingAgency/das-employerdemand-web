using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models
{
    public class WhenBuildingTheBuildRegisterDemandRequestPropertyOrderDictionary
    {
        [Test]
        public void Then_The_Dictionary_Is_Built_And_Ordered()
        {   
            //Act
            var propertyOrderedDictionary = RegisterDemandRequest.BuildPropertyOrderDictionary();
            
            //Assert
            propertyOrderedDictionary.Should().BeAssignableTo<Dictionary<string, int>>();
            propertyOrderedDictionary.Count.Should().Be(typeof(RegisterDemandRequest).Properties().Count());
        }
    }
}