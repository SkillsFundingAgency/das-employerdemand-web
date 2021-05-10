using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenValidatingCachedProviderInterest
    {
        [Test, AutoData]
        public async Task And_Has_One_Or_More_Demands_Then_Is_Valid(CreateCachedProviderInterestCommand command)
        {
            //Arrange
            var validator = new CreateProviderInterestCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeTrue();
        }

        [Test, AutoData]
        public async Task And_Has_No_Demands_Then_Not_Invalid(CreateCachedProviderInterestCommand command)
        {
            //Arrange
            command.EmployerDemands = new List<EmployerDemands>();
            var validator = new CreateProviderInterestCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.EmployerDemands));
            actual.ValidationDictionary[nameof(command.EmployerDemands)].Should()
                .Be("Select the employers you're interested in");
        }

        [Test, AutoData]
        public async Task And_Demands_Is_Null_Then_Not_Invalid(CreateCachedProviderInterestCommand command)
        {
            //Arrange
            command.EmployerDemands = null;
            var validator = new CreateProviderInterestCommandValidator();

            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.EmployerDemands));
            actual.ValidationDictionary[nameof(command.EmployerDemands)].Should()
                .Be("Select the employers you're interested in");
        }
    }
}