using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.EmployerDemand.Domain.Validation.ValidationResult;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenCreatingCachedCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Service_Called_If_Valid(
            CreateCourseDemandCommand command,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCourseDemandCommand>> validator,
            CreateCachedCourseDemandCommandHandler handler)
        {
            //Arrange
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(new ValidationResult( ));
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            service.Verify(x=>x.CreateCacheCourseDemand(command));
            actual.Id.Should().Be(command.Id);
        }

        [Test, MoqAutoData]
        public void Then_If_The_Command_Is_Not_Valid_Then_A_ValidationException_Is_Thrown(
            string propertyName,
            CreateCourseDemandCommand command,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCourseDemandCommand>> validator,
            CreateCachedCourseDemandCommandHandler handler)
        {
            //Arrange
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(new ValidationResult{ValidationDictionary = { {propertyName,""}}});
            
            //Act
            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            //Assert
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }
    }
}