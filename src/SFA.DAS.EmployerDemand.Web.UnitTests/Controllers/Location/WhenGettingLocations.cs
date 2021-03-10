using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Locations.Queries;
using SFA.DAS.EmployerDemand.Web.Controllers;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Controllers.Location
{
    public class WhenGettingLocations
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Sent_And_Data_Retrieved_And_Json_Returned(
            string searchTerm,
            GetLocationsQueryResult response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]LocationsController controller)
        {
            //Arrange
            mediator.Setup(x => 
                    x.Send(It.Is<GetLocationsQuery>(c => 
                        c.SearchTerm.Equals(searchTerm)),It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            
            //Act
            var actual = await controller.Locations(searchTerm);
            
            //Assert
            Assert.IsNotNull(actual);
            var actualResult = actual as JsonResult;
            Assert.IsNotNull(actualResult);
            var model = (LocationsViewModel)actualResult.Value;
            Assert.IsNotNull(model);
        }
    }
}