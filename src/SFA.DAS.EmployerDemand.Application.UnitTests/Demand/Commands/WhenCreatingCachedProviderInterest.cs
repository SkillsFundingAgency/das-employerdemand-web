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

        [Test, MoqAutoData]
        public async Task Then_If_The_Request_Is_Valid_And_No_Cached_Objed_Exists_The_Item_Is_Added_To_The_Cache(
            CreateCachedProviderInterestCommand command,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCachedProviderInterestCommand>> validator,
            CreateCachedProviderInterestCommandHandler handler)
        {
            //Arrange
            validator
                .Setup(x=>x.ValidateAsync(command))
                .ReturnsAsync(new ValidationResult());

            service
                .Setup(x => x.GetCachedProviderInterest(command.Id))
                .ReturnsAsync((IProviderDemandInterest) null);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            service.Verify(x=>x.CreateCachedProviderInterest(command), Times.Once);
            actual.Id.Should().Be(command.Id);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Request_Is_Valid_And_Cached_Object_Exists_EmployerDemands_Are_Updated_In_The_Cache(
            CreateCachedProviderInterestCommand command,
            IProviderDemandInterest providerInterest,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCachedProviderInterestCommand>> validator,
            CreateCachedProviderInterestCommandHandler handler)
        {
            //Arrange
            var expectedCachedObject = providerInterest;
            expectedCachedObject.EmployerDemands = command.EmployerDemands;

            validator
                .Setup(x => x.ValidateAsync(command))
                .ReturnsAsync(new ValidationResult());

            service
                .Setup(x => x.GetCachedProviderInterest(command.Id))
                .ReturnsAsync(providerInterest);

            //Act
            var actual = await handler.Handle(command, CancellationToken.None);

            //Assert
            service.Verify(x => x.CreateCachedProviderInterest(expectedCachedObject), Times.Once);
            service.Verify(x => x.CreateCachedProviderInterest(command), Times.Never);
            actual.Id.Should().Be(expectedCachedObject.Id);
        }
    }
}