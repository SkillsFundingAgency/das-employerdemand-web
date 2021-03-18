using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand;
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
        [Route("course/{id}/enter-apprenticeship-details/", Name = RouteNames.RegisterDemand)]
        public async Task<IActionResult> RegisterDemand(int id, [FromQuery] Guid? createDemandId)
        {
            var result = await _mediator.Send(new GetCreateCourseDemandQuery {TrainingCourseId = id, CreateDemandId = createDemandId});

            var model = (RegisterCourseDemandViewModel) result.CourseDemand;
            
            return View(model);
        }

        [HttpPost]
        [Route("course/{id}/enter-apprenticeship-details", Name = RouteNames.PostRegisterDemand)]
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
                    Id = !request.CreateDemandId.HasValue || request.CreateDemandId == Guid.Empty ? 
                        Guid.NewGuid() : request.CreateDemandId.Value,
                    Location = request.Location,
                    OrganisationName = request.OrganisationName,
                    ContactEmailAddress = request.ContactEmailAddress,
                    NumberOfApprentices = request.NumberOfApprentices,
                    TrainingCourseId = request.TrainingCourseId,
                    NumberOfApprenticesKnown = request.NumberOfApprenticesKnown
                });

                return RedirectToRoute(RouteNames.ConfirmRegisterDemand, new
                {
                    createDemandId = createResult.Id,
                    Id = request.TrainingCourseId
                });
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
        [Route("course/{id}/confirm-apprenticeship-details/{createDemandId}", Name = RouteNames.ConfirmRegisterDemand)]
        public async Task<IActionResult> ConfirmRegisterDemand(int courseId, Guid id)
        {
            var result = await _mediator.Send(new GetCachedCreateCourseDemandQuery {Id = id});

            var model = (ConfirmCourseDemandViewModel) result.CourseDemand;

            if (model == null)
            {
                return RedirectToRoute(RouteNames.RegisterDemand, new {Id = courseId});
            }
           
            return View(model);
        }
        
        private async Task<RegisterCourseDemandViewModel> BuildRegisterCourseDemandViewModelFromPostRequest(
            RegisterDemandRequest request)
        {
            var model = (RegisterCourseDemandViewModel) request;

            var result = await _mediator.Send(new GetCreateCourseDemandQuery {TrainingCourseId = request.TrainingCourseId});
            model.TrainingCourse = result.CourseDemand.Course;
            return model;
        }
    }
}