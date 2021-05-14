using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Demand
{
    public class WhenPostingConfirmRegisterDemand
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Is_Called_With_Id_And_Encoded_Id_And_Redirected_To_ConfirmEmail(
            Guid demandId,
            int trainingCourseId,
            string encodedDemand,
            [Frozen] Mock<IDataProtector> protector,
            [Frozen] Mock<IDataProtectionProvider> provider,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] RegisterDemandController controller)
        {
            //Act
            var toEncode = WebEncoders.Base64UrlDecode(demandId.ToString());
            provider.Setup(x => x.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName)).Returns(protector.Object);
            protector.Setup(c => c.Protect(It.Is<byte[]>(
                x => x[0].Equals(Encoding.UTF8.GetBytes($"{demandId}")[0])))).Returns(toEncode);
            var actual = await controller.PostConfirmRegisterDemand(trainingCourseId, demandId) as RedirectToRouteResult;
            
            //Assert
            mediator.Verify(x =>
                x.Send(It.Is<CreateCourseDemandCommand>(c => 
                        c.Id == demandId
                        && c.EncodedId == WebEncoders.Base64UrlEncode(toEncode)
                    ), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(actual);
            actual.RouteName.Should().Be(RouteNames.ConfirmEmployerDemandEmail);
            actual.RouteValues["id"].Should().Be(trainingCourseId);
            actual.RouteValues["createDemandId"].Should().Be(demandId);
        }
    }
}