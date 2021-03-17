using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
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
            CreateCourseDemandCommand command,
            GetCreateCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCourseDemandCommand>> validator,
            CreateCachedCourseDemandCommandHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetCreateCourseDemand(command.TrainingCourseId, command.Location))
                .ReturnsAsync(response);
            validator.Setup(x=>x.ValidateAsync(command)).ReturnsAsync(new ValidationResult( ));
            
            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            service.Verify(x=>x.CreateCacheCourseDemand(It.Is<CreateCourseDemandCommand>(c=>
                    c.LocationItem.Name.Equals(response.Location.Name)
                    && c.LocationItem.LocationPoint.Equals(response.Location.LocationPoint)
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
        public void Then_The_Api_Is_Called_With_Course_And_Location_And_If_No_Location_Validation_Error_Returned(
            CreateCourseDemandCommand command,
            GetCreateCourseDemandResponse response,
            [Frozen] Mock<IDemandService> service,
            [Frozen] Mock<IValidator<CreateCourseDemandCommand>> validator,
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
            service.Verify(x=>x.CreateCacheCourseDemand(It.IsAny<CreateCourseDemandCommand>()), Times.Never);
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{nameof(command.Location)}|Enter a real town, city or postcode*");
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