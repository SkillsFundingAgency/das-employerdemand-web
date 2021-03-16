using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerDemand.Application.Locations.Queries;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.Controllers
{
    [Route("[controller]")]
    public class LocationsController : Controller
    {
        private readonly IMediator _mediator;

        public LocationsController (IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Locations([FromQuery]string searchTerm)
        {
            var result = await _mediator.Send(new GetLocationsQuery
            {
                SearchTerm = searchTerm
            });

            var model = new LocationsViewModel
            {
                Locations = result.LocationItems.Select(c=>(LocationViewModel)c).ToList()
            };
            
            return new JsonResult(model);
        }
    }
}