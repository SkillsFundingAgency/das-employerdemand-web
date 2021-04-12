using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.ProviderDemand
{
    public class WhenPostingProviderEmployerOpportunitiesForCourse
    {
        [Test, MoqAutoData]
        public async Task And_There_Is_A_Validation_Exception_Then_Register_View_Is_Returned(
            ProviderRegisterInterestRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mediator
                .Setup(x => x.Send(
                    It.IsAny<CreateCachedProviderInterestCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException());

            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("FindApprenticeshipTrainingOpportunitiesForCourse");
        }
    }
}