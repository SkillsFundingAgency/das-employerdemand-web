using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerDemand.Application.Demand.Queries;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.Controllers
{
    [Route("[controller]")]
    public class RegisterDemandController : Controller
    {
        private readonly IMediator _mediator;

        public RegisterDemandController (IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("enter-apprenticeship-details/{id}", Name = RouteNames.RegisterDemand)]
        public async Task<IActionResult> RegisterDemand(int id)
        {
            var result = await _mediator.Send(new GetCreateCourseDemandQuery {TrainingCourseId = id});

            var model = new  RegisterCourseDemandViewModel
            {
                TrainingCourse = result.TrainingCourse
            };

            return View(model);
        }

    }
}