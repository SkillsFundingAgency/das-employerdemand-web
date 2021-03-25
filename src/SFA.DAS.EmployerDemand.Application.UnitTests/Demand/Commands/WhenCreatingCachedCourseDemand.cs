using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.EmployerDemand.Domain.Validation.ValidationResult;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenCreatingCachedCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Service_Called_If_Valid_With_Location_Information(
            CreateCachedCourseDemandCommand command,
            GetCreateCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCachedCourseDemandCommand>> validator,
            CreateCachedCourseDemandCommandHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetCreateCourseDemand(command.TrainingCourseId, command.Location))
                .ReturnsAsync(response);
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(new ValidationResult( ));
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            service.Verify(x=>x.CreateCacheCourseDemand(It.Is<CreateCachedCourseDemandCommand>(c=>
                    c.LocationItem.Name.Equals(response.Location.Name)
                    && c.LocationItem.LocationPoint.Equals(response.Location.LocationPoint.GeoPoint)
                    && c.OrganisationName.Equals(command.OrganisationName)
                    && c.ContactEmailAddress.Equals(command.ContactEmailAddress)
                    && c.NumberOfApprentices.Equals(command.NumberOfApprentices)
                    && c.NumberOfApprenticesKnown.Equals(command.NumberOfApprenticesKnown)
                    && c.TrainingCourseId.Equals(command.TrainingCourseId)
                    && c.Course.Id.Equals(command.Course.Id)
                    && c.Course.Level.Equals(command.Course.Level)
                    && c.Course.Title.Equals(command.Course.Title)
                    ))
                , Times.Once);
            actual.Id.Should().Be(command.Id);
        }

        [Test, MoqAutoData]
        public void Then_The_Api_Is_Called_With_Course_And_Location_And_If_No_Location_Found_Validation_Error_Returned(
            CreateCachedCourseDemandCommand command,
            GetCreateCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCachedCourseDemandCommand>> validator,
            CreateCachedCourseDemandCommandHandler handler)
        {
            //Arrange
            response.Location = null;
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(new ValidationResult( ));
            service.Setup(x => x.GetCreateCourseDemand(command.TrainingCourseId, command.Location))
                .ReturnsAsync(response);
            
            //Act
            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            //Assert
            service.Verify(x=>x.CreateCacheCourseDemand(It.IsAny<CreateCachedCourseDemandCommand>()), Times.Never);
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{nameof(command.Location)}|Enter a real town, city or postcode*");
        }

        [Test, MoqAutoData]
        public void Then_If_There_Are_Validation_Errors_But_Not_A_Location_Error_Then_The_Service_Is_Still_Called(
            CreateCachedCourseDemandCommand command,
            GetCreateCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCachedCourseDemandCommand>> validator,
            CreateCachedCourseDemandCommandHandler handler)
        {
            //Arrange
            response.Location = null;
            var validationResult = new ValidationResult();
            validationResult.AddError(nameof(command.OrganisationName));
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(validationResult);
            service.Setup(x => x.GetCreateCourseDemand(command.TrainingCourseId, command.Location))
                .ReturnsAsync(response);
            
            //Act
            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            //Assert
            service.Verify(x=>x.CreateCacheCourseDemand(It.IsAny<CreateCachedCourseDemandCommand>()), Times.Never);
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{nameof(command.Location)}|Enter a real town, city or postcode*");
        }

        [Test, MoqAutoData]
        public void Then_If_The_Command_Is_Not_Valid_Then_A_ValidationException_Is_Thrown(
            string propertyName,
            CreateCachedCourseDemandCommand command,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCachedCourseDemandCommand>> validator,
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
        
        [Test, MoqAutoData]
        public void Then_If_The_Command_Is_Not_Valid_And_Has_A_Location_Error_Then_A_ValidationException_Is_Thrown_And_The_Service_Not_Called(
            string propertyName,
            CreateCachedCourseDemandCommand command,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCachedCourseDemandCommand>> validator,
            CreateCachedCourseDemandCommandHandler handler)
        {
            //Arrange
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(new ValidationResult{ValidationDictionary = { {nameof(command.Location),""}}});
            
            //Act
            var act = new Func<Task>(async () => await handler.Handle(command, CancellationToken.None));

            //Assert
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{nameof(command.Location)}*");
            service.Verify(x => x.GetCreateCourseDemand(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }
    }
}