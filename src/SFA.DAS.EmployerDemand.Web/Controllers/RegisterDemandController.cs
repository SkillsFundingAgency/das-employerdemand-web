using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCourseDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerCourseDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCreateCourseDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnverifiedEmployerCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Configuration;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.Controllers
{
    [Route("[controller]")]
    public class RegisterDemandController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RegisterDemandController> _logger;
        private readonly Domain.Configuration.EmployerDemand _config;
        private readonly IDataProtector _employerDemandDataProtector;

        public RegisterDemandController (
            IMediator mediator, 
            IOptions<Domain.Configuration.EmployerDemand> config,
            IDataProtectionProvider provider,
            ILogger<RegisterDemandController> logger)
        {
            _mediator = mediator;
            _logger = logger;
            _config = config.Value;
            _employerDemandDataProtector = provider.CreateProtector(EmployerDemandConstants.EmployerDemandProtectorName);
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
                
                var createResult = await _mediator.Send(new CreateCachedCourseDemandCommand
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
        [Route("course/{id}/check-answers", Name = RouteNames.ConfirmRegisterDemand)]
        public async Task<IActionResult> ConfirmRegisterDemand(int id, [FromQuery] Guid createDemandId)
        {
            var result = await _mediator.Send(new GetCachedCreateCourseDemandQuery {Id = createDemandId});

            var model = (ConfirmCourseDemandViewModel) result.CourseDemand;

            if (model == null)
            {
                return RedirectToRoute(RouteNames.RegisterDemand, new {Id = id});
            }
           
            return View(model);
        }

        [HttpPost]
        [Route("course/{id}/check-answers", Name = RouteNames.PostConfirmRegisterDemand)]
        public async Task<IActionResult> PostConfirmRegisterDemand(int id, Guid createDemandId)
        {
            var encodedId = WebEncoders.Base64UrlEncode(_employerDemandDataProtector.Protect(
                System.Text.Encoding.UTF8.GetBytes($"{createDemandId}")));
            await _mediator.Send(new CreateCourseDemandCommand
            {
                Id = createDemandId,
                EncodedId = encodedId
            });

#if DEBUG
            _logger.LogInformation($"confirm page at https://localhost:5011/registerdemand/course/14/complete?demandId={encodedId}");
#endif
            
            return RedirectToRoute(RouteNames.ConfirmEmployerDemandEmail, new {Id = id, CreateDemandId = createDemandId});
        }

        [HttpGet]
        [Route("course/{id}/complete", Name = RouteNames.RegisterDemandCompleted)]
        public async Task<IActionResult> RegisterDemandCompleted(int id, [FromQuery] string demandId)
        {
            var decodedDemandId = GetEncodedDemandId(demandId);

            if (!decodedDemandId.HasValue)
            {
                return RedirectToRoute(RouteNames.RegisterDemand, new {Id = id});
            }
            
            var result = await _mediator.Send(new VerifyEmployerCourseDemandCommand {Id = decodedDemandId.Value});
            
            var model = (CompletedCourseDemandViewModel) result.EmployerDemand;

            if (model == null)
            {
                return RedirectToRoute(RouteNames.RegisterDemand, new {Id = id});
            }


            model.FindApprenticeshipTrainingCourseUrl = _config.FindApprenticeshipTrainingUrl + "/courses";
           
            return View(model);
        }

        [HttpGet]
        [Route("course/{id}/verify-email", Name= RouteNames.ConfirmEmployerDemandEmail)]
        public async Task<IActionResult> VerifyEmployerDemandEmail(int id, [FromQuery] Guid createDemandId)
        {
            var result = await _mediator.Send(new GetUnverifiedEmployerCourseDemandQuery
            {
                Id = createDemandId
            });

            var model = (VerifyEmployerCourseDemandViewModel) result.CourseDemand;

            if (model == null)
            {
                return RedirectToRoute(RouteNames.RegisterDemand, new {Id = id});
            }

            if (model.Verified)
            {
                return RedirectToRoute(RouteNames.RegisterDemandCompleted, new {Id = id});
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
        
        private Guid? GetEncodedDemandId(string encodedId)
        {
            try
            {
                var base64EncodedBytes = WebEncoders.Base64UrlDecode(encodedId);
                var encodedDemandId = System.Text.Encoding.UTF8.GetString(_employerDemandDataProtector.Unprotect(base64EncodedBytes));
                var result = Guid.TryParse(encodedDemandId, out var demandId);
                return result ? demandId : (Guid?) null;
            }
            catch (FormatException e)
            {
                _logger.LogInformation(e,"Unable to decode employer demand id from request");
            }
            catch (CryptographicException e)
            {
                _logger.LogInformation(e, "Unable to decode employer demand id from request");
            }

            return null;
        }
    }
}