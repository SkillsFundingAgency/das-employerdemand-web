using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerCourseDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCreateCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenViewingStoppedInterest
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_And_The_ViewModel_Returned(
            Guid employerDemandId,
            string encodedEmployerDemandId,
            StopEmployerCourseDemandResult mediatorResult,
            [Frozen] Mock<IDataEncryptDecryptService> dataEncryptDecryptService,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            dataEncryptDecryptService.Setup(x => x.DecodeData(encodedEmployerDemandId)).Returns(employerDemandId);
            mockMediator
                .Setup(x => x.Send(
                    It.Is<StopEmployerCourseDemandCommand>(c => c.EmployerDemandId == employerDemandId)
                        , It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            //Act
            var actual = await controller.StoppedInterest(encodedEmployerDemandId) as ViewResult;
            
            //Assert
            var actualModel = actual!.Model as StoppedInterestViewModel;
            actualModel!.EmployerEmail.Should().Be(mediatorResult.EmployerEmail);
            actualModel.FatUrl.Should().Be(mockOptions.Object.Value.FindApprenticeshipTrainingUrl);
        }

        [Test, MoqAutoData]
        public async Task And_Id_Not_Parseable_Then_Redirect_To_FAT(
            string encodedEmployerDemandId,
            [Frozen] Mock<IDataEncryptDecryptService> dataEncryptDecryptService,
            [Frozen] Mock<IOptions<Domain.Configuration.EmployerDemand>> mockOptions,
            [Greedy] RegisterDemandController controller)
        {
            //Arrange
            dataEncryptDecryptService.Setup(x => x.DecodeData(encodedEmployerDemandId)).Returns((Guid?)null);
            
            //Act
            var actual = await controller.StoppedInterest(encodedEmployerDemandId) as RedirectResult;
            
            //Assert
            actual.Url.Should().Be(mockOptions.Object.Value.FindApprenticeshipTrainingUrl);
        }
    }
}