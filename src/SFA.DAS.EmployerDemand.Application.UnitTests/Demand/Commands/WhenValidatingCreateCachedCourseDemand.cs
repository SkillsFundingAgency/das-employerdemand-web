using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenValidatingCreateCachedCourseDemand
    {
        [Test, AutoData]
        public async Task Then_If_All_Fields_Are_Present_The_Command_Is_Valid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            command.ContactEmailAddress = $"{command.ContactEmailAddress}@test.com";
            command.NumberOfApprenticesKnown = false;
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeTrue();
        }

        [Test, AutoData]
        public async Task Then_If_There_Is_No_OrganisationName_It_Is_Invalid(CreateCachedCourseDemandCommand command)
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
            actual.ValidationDictionary[nameof(command.OrganisationName)].Should()
                .Be("Enter the name of the organisation");
        }
        
        [Test, AutoData]
        public async Task Then_If_There_Is_No_TrainingCourseId_It_Is_Invalid(CreateCachedCourseDemandCommand command)
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
        public async Task Then_If_There_Is_No_ContactEmailAddress_It_Is_Invalid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            command.ContactEmailAddress = string.Empty;
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.ContactEmailAddress));
            actual.ValidationDictionary[nameof(command.ContactEmailAddress)].Should()
                .Be("Enter an email address");
        }
        
        [Test, AutoData]
        public async Task Then_If_There_Is_Not_A_Valid_ContactEmailAddress_It_Is_Invalid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.ContactEmailAddress));
            actual.ValidationDictionary[nameof(command.ContactEmailAddress)].Should()
                .Be("Enter an email address in the correct format, like name@example.com");
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Value_For_NumberOfApprenticesKnown_Then_Invalid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            command.NumberOfApprenticesKnown = null;
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.NumberOfApprenticesKnown));
            actual.ValidationDictionary[nameof(command.NumberOfApprenticesKnown)].Should()
                .Be("Select yes if you know how many apprentices will take this apprenticeship training");
        }

        [Test, AutoData]
        public async Task Then_If_The_NumberOfApprenticesIsKnown_Is_True_Then_The_Number_Of_Apprentices_Is_Not_Greater_Than_One_Then_Invalid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            command.NumberOfApprenticesKnown = true;
            command.NumberOfApprentices = "0";
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
        public async Task Then_If_The_NumberOfApprenticesIsKnown_Is_True_And_The_Number_Of_Apprentices_Is_Less_Than_Zero_Then_Invalid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            command.NumberOfApprenticesKnown = true;
            command.NumberOfApprentices = "-1";
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.NumberOfApprentices));
            actual.ValidationDictionary[nameof(command.NumberOfApprentices)].Should()
                .Be("Number of apprentices must be 1 or more");
        }

        [Test, AutoData]
        public async Task Then_If_The_NumberOfApprenticesIsKnown_Is_True_And_The_Number_Of_Apprentices_Is_Greater_Than_9999_Then_Invalid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            command.NumberOfApprenticesKnown = true;
            command.NumberOfApprentices = "10000";
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.NumberOfApprentices));
            actual.ValidationDictionary[nameof(command.NumberOfApprentices)].Should()
                .Be("Number of apprentices must be 9999 or less");
        }
        
        
        [Test, AutoData]
        public async Task Then_If_The_NumberOfApprenticesIsKnown_Is_True_And_The_Number_Of_Apprentices_Is_Not_An_Int_Then_Invalid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            command.NumberOfApprenticesKnown = true;
            command.NumberOfApprentices = $"{long.MaxValue}";
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.NumberOfApprentices));
            actual.ValidationDictionary[nameof(command.NumberOfApprentices)].Should()
                .Be("Number of apprentices must be 9999 or less");
        }
        
        [Test, AutoData]
        public async Task Then_If_The_NumberOfApprenticesIsKnown_Is_True_Then_The_Number_Of_Apprentices_Is_Null_Then_Invalid(CreateCachedCourseDemandCommand command)
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

        [Test, AutoData]
        public async Task Then_If_No_Address_Marked_As_Invalid(CreateCachedCourseDemandCommand command)
        {
            //Arrange
            command.Location = string.Empty;
            command.ContactEmailAddress = $"{command.ContactEmailAddress}@test.com";
            var validator = new CreateCourseDemandCommandValidator();
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary.Should().ContainKey(nameof(command.Location));
            actual.ValidationDictionary[nameof(command.Location)].Should()
                .Be("Enter a town, city or postcode");
        }
    }
}