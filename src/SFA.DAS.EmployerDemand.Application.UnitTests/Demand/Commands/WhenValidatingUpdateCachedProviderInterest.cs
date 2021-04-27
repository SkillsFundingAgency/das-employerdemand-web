using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.UpdateCachedProviderInterest;

namespace SFA.DAS.EmployerDemand.Application.UnitTests.Demand.Commands
{
    public class WhenValidatingUpdateCachedProviderInterest
    {
        
        [Test, AutoData]
        public async Task And_Has_No_PhoneNumber_Then_Not_Valid(UpdateCachedProviderInterestCommand command)
        {
            //Arrange
            var validator = new UpdateCachedProviderInterestCommandValidator();
            command.EmailAddress = "test@test.com";
            command.PhoneNumber = "";
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary[nameof(command.PhoneNumber)].Should().Be("Enter a telephone number");
            actual.ValidationDictionary.Count.Should().Be(1);
        }
        
        [Test, AutoData]
        public async Task And_Has_An_Invalid_PhoneNumber_Then_Not_Valid(UpdateCachedProviderInterestCommand command)
        {
            //Arrange
            var validator = new UpdateCachedProviderInterestCommandValidator();
            command.EmailAddress = "test@test.com";
            command.PhoneNumber = "abc123";
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary[nameof(command.PhoneNumber)].Should().Be("Enter a telephone number, like 01632 960 001, 07700 900 982 or +44 0808 157 0192");
            actual.ValidationDictionary.Count.Should().Be(1);
        }
        
        [Test, AutoData]
        public async Task And_Has_No_Email_Then_Not_Valid(UpdateCachedProviderInterestCommand command)
        {
            //Arrange
            var validator = new UpdateCachedProviderInterestCommandValidator();
            command.EmailAddress = "";
            command.PhoneNumber = "+44 0808 157 0192";
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary[nameof(command.EmailAddress)].Should().Be("Enter an email address");
            actual.ValidationDictionary.Count.Should().Be(1);
        }
        [Test, AutoData]
        public async Task And_Has_An_Invalid_Email_Then_Not_Valid(UpdateCachedProviderInterestCommand command)
        {
            //Arrange
            var validator = new UpdateCachedProviderInterestCommandValidator();
            command.EmailAddress = "test";
            command.PhoneNumber = "07700 900 982";
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeFalse();
            actual.ValidationDictionary[nameof(command.EmailAddress)].Should().Be("Enter an email address in the correct format, like name@example.com");
            actual.ValidationDictionary.Count.Should().Be(1);
        }

        [Test, AutoData]
        public async Task And_Has_All_Required_Fields_In_The_Correct_Format_Then_Valid(UpdateCachedProviderInterestCommand command)
        {
            //Arrange
            var validator = new UpdateCachedProviderInterestCommandValidator();
            command.EmailAddress = "test@test.com";
            command.PhoneNumber = "01632 960 001";
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().BeTrue();
        }

        [Test]
        [InlineAutoData("010 5656 5656 ( Option: 1 )", true)]
        [InlineAutoData("+ 44 (0)111 011 1110 ( Option: 1 )", true)]
        [InlineAutoData("+ 44 (0)111 011 1110 ( OPTION: 1 )", true)]
        [InlineAutoData("010 1010 1100 ( ext: 1000 ) ", true)]
        [InlineAutoData("01012 111555 Ext 100/100", true)]
        [InlineAutoData("01012 111555 EXT 100/100", true)]
        [InlineAutoData("01012 111555 ext 100/100", true)]
        [InlineAutoData("01012 111555#100", true)]
        [InlineAutoData("01012 111555 100/100", true)]
        [InlineAutoData("01112223443 / 01112223443", true)]
        [InlineAutoData("01112223443 / 01112223443 123456789012341234567891", true)]
        [InlineAutoData("01112223443 / 01112223443 1234567890123412345678912", false)]
        [InlineAutoData("01112223443 / 01112223443 abc", false)]
        public async Task Then_Validates_PhoneNumbers_Correctly(string phoneNumber, bool expected, UpdateCachedProviderInterestCommand command)
        {
            //Arrange
            var validator = new UpdateCachedProviderInterestCommandValidator();
            command.EmailAddress = "test@test.com";
            command.PhoneNumber = phoneNumber;
            
            //Act
            var actual = await validator.ValidateAsync(command);

            //Assert
            actual.IsValid().Should().Be(expected);
        }
    }
}