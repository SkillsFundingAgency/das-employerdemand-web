using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCreateCourseDemand;
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

        [HttpPost]
        [Route("enter-apprenticeship-details/{id}", Name = RouteNames.PostRegisterDemand)]
        public async Task<IActionResult> PostRegisterDemand(RegisterDemandRequest request)
        {
            try
            {
                if (request.NumberOfApprenticesKnown.HasValue && !request.NumberOfApprenticesKnown.Value)
                {
                    request.NumberOfApprentices = string.Empty;
                }
                
                var createResult = await _mediator.Send(new CreateCourseDemandCommand
                {
                    Id = Guid.NewGuid(),
                    Location = request.Location,
                    OrganisationName = request.OrganisationName,
                    ContactEmailAddress = request.ContactEmailAddress,
                    NumberOfApprentices = request.NumberOfApprentices,
                    TrainingCourseId = request.TrainingCourseId,
                    NumberOfApprenticesKnown = request.NumberOfApprenticesKnown
                });

                return RedirectToRoute(RouteNames.ConfirmRegisterDemand, new { createResult.Id });
            }
            catch (ValidationException e)
            {
                foreach (var member in e.ValidationResult.MemberNames)
                {
                    ModelState.AddModelError(member.Split('|')[0], member.Split('|')[1]);
                }
                var model = await BuildRegisterCourseDemandViewModelFromPostRequest(request);
                
                return View("RegisterDemand", model);
            }
            
        }

        [HttpGet]
        [Route("confirm-apprenticeship-details/{id}", Name = RouteNames.ConfirmRegisterDemand)]
        public IActionResult ConfirmRegisterDemand(Guid id)
        {
            return View();
        }

        private async Task<RegisterCourseDemandViewModel> BuildRegisterCourseDemandViewModelFromPostRequest(
            RegisterDemandRequest request)
        {
            var model = (RegisterCourseDemandViewModel) request;

            var result = await _mediator.Send(new GetCreateCourseDemandQuery {TrainingCourseId = request.TrainingCourseId});
            model.TrainingCourse = result.TrainingCourse;
            return model;
        }
    }
}