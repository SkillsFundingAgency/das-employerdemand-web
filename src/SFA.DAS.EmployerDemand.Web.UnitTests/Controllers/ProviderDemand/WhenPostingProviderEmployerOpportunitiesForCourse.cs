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
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;
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
            GetProviderEmployerDemandDetailsQueryResult resultForGet,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] HomeController controller)
        {
            //Arrange
            mockMediator
                .Setup(x => x.Send(
                    It.IsAny<CreateCachedProviderInterestCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException());
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetProviderEmployerDemandDetailsQuery>(c =>
                        c.Ukprn.Equals(request.Ukprn) 
                        && c.CourseId.Equals(request.CourseId) 
                        && c.Location.Equals(request.Location)
                        && c.LocationRadius.Equals(request.Radius)), 
                    CancellationToken.None))
                .ReturnsAsync(resultForGet);

            //Act
            var actual = await controller.PostFindApprenticeshipTrainingOpportunitiesForCourse(request) as ViewResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.ViewName.Should().Be("FindApprenticeshipTrainingOpportunitiesForCourse");
            var model = actual.Model as AggregatedProviderCourseDemandDetailsViewModel;
            model.SelectedEmployerDemandIds.Should().BeEquivalentTo(request.EmployerDemandIds);
            model.Should().BeEquivalentTo((AggregatedProviderCourseDemandDetailsViewModel)resultForGet, options => options.Excluding(viewModel => viewModel.SelectedEmployerDemandIds));
        }
    }
}