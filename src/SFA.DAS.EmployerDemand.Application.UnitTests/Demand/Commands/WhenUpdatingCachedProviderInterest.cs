using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.UpdateCachedProviderInterest;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.EmployerDemand.Domain.Validation.ValidationResult;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenUpdatingCachedProviderInterest
    {
        [Test, MoqAutoData]
        public void Then_If_The_Command_Is_Not_Valid_Then_A_ValidationException_Is_Thrown(
            string propertyName,
            UpdateCachedProviderInterestCommand command,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<UpdateCachedProviderInterestCommand>> validator,
            UpdateCachedProviderInterestCommandHandler handler)
        {
            //Arrange
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(new ValidationResult{ValidationDictionary = { {propertyName,""}}});
            
            //Act
            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            //Assert
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Command_Is_Valid_The_Cached_Object_Is_Updated(
            ProviderInterestRequest response,
            UpdateCachedProviderInterestCommand command,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<UpdateCachedProviderInterestCommand>> validator,
            UpdateCachedProviderInterestCommandHandler handler)
        {
            //Arrange
            response.Id = command.Id;
            service.Setup(x => x.GetCachedProviderInterest(command.Id))
                .ReturnsAsync(response);
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(new ValidationResult( ));
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            service.Verify(x=>x.CreateCachedProviderInterest(It.Is<IProviderDemandInterest>(c=>
                    c.Id.Equals(command.Id)
                    && c.Website.Equals(command.Website)
                    && c.PhoneNumber.Equals(command.PhoneNumber)
                    && c.EmailAddress.Equals(command.EmailAddress)
                    && c.Course.Id.Equals(response.Course.Id)
                    && c.Course.Level.Equals(response.Course.Level)
                    && c.Course.Title.Equals(response.Course.Title)
                    && c.EmployerDemandIds.Count() == response.EmployerDemandIds.Count()
                ))
                , Times.Once);
            actual.Id.Should().Be(command.Id);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Valid_But_Cache_Has_Expired_Null_Returned(
            UpdateCachedProviderInterestCommand command,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<UpdateCachedProviderInterestCommand>> validator,
            UpdateCachedProviderInterestCommandHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetCachedProviderInterest(command.Id))
                .ReturnsAsync((ProviderInterestRequest) null);
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.Id.Should().BeNull();
        }
    }
}