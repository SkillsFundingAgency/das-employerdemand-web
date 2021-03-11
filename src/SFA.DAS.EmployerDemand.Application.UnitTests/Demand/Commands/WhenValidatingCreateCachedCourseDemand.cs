using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenValidatingCreateCachedCourseDemand
    {
        [Test, AutoData]
        public async Task Then_If_All_Fields_Are_Present_The_Command_Is_Valid(CreateCourseDemandCommand command)
        {
            //Arrange
            command.ContactEmailAddress = $"{command.ContactEmailAddress}@test.com";
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeTrue();
        }

        [Test, AutoData]
        public async Task Then_If_There_Is_No_OrganisationName_It_Is_Invalid(CreateCourseDemandCommand command)
        {
            //Arrange
            command.OrganisationName = string.Empty;
            command.ContactEmailAddress = $"{command.ContactEmailAddress}@test.com";
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.OrganisationName));
        }
        
        [Test, AutoData]
        public async Task Then_If_There_Is_No_TrainingCourseId_It_Is_Invalid(CreateCourseDemandCommand command)
        {
            //Arrange
            command.TrainingCourseId = 0;
            command.ContactEmailAddress = $"{command.ContactEmailAddress}@test.com";
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.TrainingCourseId));
        }
        
        [Test, AutoData]
        public async Task Then_If_There_Is_No_ContactEmailAddress_It_Is_Invalid(CreateCourseDemandCommand command)
        {
            //Arrange
            command.ContactEmailAddress = string.Empty;
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.ContactEmailAddress));
        }
        
        [Test, AutoData]
        public async Task Then_If_There_Is_Not_A_Valid_ContactEmailAddress_It_Is_Invalid(CreateCourseDemandCommand command)
        {
            //Arrange
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.ContactEmailAddress));
        }

        [Test, AutoData]
        public async Task Then_If_The_NumberOfApprenticesIsKnown_Is_True_Then_The_Number_Of_Apprentices_Is_Not_Greater_Than_One_Then_Invalid(CreateCourseDemandCommand command)
        {
            //Arrange
            command.NumberOfApprenticesKnown = true;
            command.NumberOfApprentices = 0;
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.NumberOfApprentices));
            actual.ValidationDictionary[nameof(command.NumberOfApprentices)].Should()
                .Be("Enter the number of apprentices");
        }
        [Test, AutoData]
        public async Task Then_If_The_NumberOfApprenticesIsKnown_Is_True_Then_The_Number_Of_Apprentices_Is_Null_Then_Invalid(CreateCourseDemandCommand command)
        {
            //Arrange
            command.NumberOfApprenticesKnown = true;
            command.NumberOfApprentices = null;
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.NumberOfApprentices));
            actual.ValidationDictionary[nameof(command.NumberOfApprentices)].Should()
                .Be("Enter the number of apprentices");
        }
    }
}