using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.EmployerDemand.Domain.Validation.ValidationResult;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenCreatingCachedProviderInterest
    {
        [Test, MoqAutoData]
        public void And_The_Command_Is_Not_Valid_Then_A_ValidationException_Is_Thrown(
            string propertyName,
            CreateCachedProviderInterestCommand command,
            [Frozen] Mock<IValidator<CreateCachedProviderInterestCommand>> validator,
            CreateCachedProviderInterestCommandHandler handler)
        {
            //Arrange
            validator
                .Setup(x=>x.ValidateAsync(command))
                .ReturnsAsync(new ValidationResult{ValidationDictionary = { {propertyName,""}}});
            
            //Act
            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            //Assert
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }
    }
}