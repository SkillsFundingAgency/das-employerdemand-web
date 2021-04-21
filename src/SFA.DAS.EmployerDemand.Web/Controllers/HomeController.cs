using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Infrastructure.Authorization;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Domain.Configuration.EmployerDemand _config;

        public HomeController (IMediator mediator, IOptions<Domain.Configuration.EmployerDemand> config)
        {
            _mediator = mediator;
            _config = config.Value;
        }
        
        [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
        [Route("start", Name = RouteNames.ServiceStart, Order = 1)]
        public IActionResult Index()
        {
            var ukprn = HttpContext.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;

            return new RedirectToRouteResult(RouteNames.ProviderDashboard, new {ukprn});
        }
        
        [Route("{ukprn}/find-apprenticeship-opportunities", Name = RouteNames.ProviderDashboard)]
        public async Task<IActionResult> FindApprenticeshipTrainingOpportunities(int ukprn, [FromQuery]int? selectedCourseId, [FromQuery]string location, [FromQuery]string radius)
        {
            var result = await _mediator.Send(new GetProviderEmployerDemandQuery
            {
                Ukprn = ukprn,
                CourseId = selectedCourseId,
                Location = location,
                LocationRadius = radius
            });

            var model = (AggregatedProviderCourseDemandViewModel) result;
            ViewData["ProviderDashboard"] = _config.ProviderPortalUrl;
            return View(model);
        }

        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}", Name = RouteNames.ProviderDemandDetails)]
        public async Task<IActionResult> FindApprenticeshipTrainingOpportunitiesForCourse(
            int ukprn, 
            int courseId, 
            [FromQuery]string location, 
            [FromQuery]string radius)
        {
            var model = await BuildAggregatedProviderCourseDemandDetailsViewModel(
                ukprn,
                courseId,
                location,
                radius);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}", Name = RouteNames.PostProviderDemandDetails)]
        public async Task<IActionResult> PostFindApprenticeshipTrainingOpportunitiesForCourse(ProviderRegisterInterestRequest request)
        {
            try
            {
                var result = await _mediator.Send(new CreateCachedProviderInterestCommand
                {
                    Ukprn = request.Ukprn,
                    EmployerDemandIds = request.EmployerDemandIds
                });

                return RedirectToRoute(RouteNames.ProviderDashboard, new {request.Ukprn});
            }
            catch (ValidationException e)
            {
                foreach (var member in e.ValidationResult.MemberNames)
                {
                    ModelState.AddModelError(member.Split('|')[0], member.Split('|')[1]);
                }

                var model = await BuildAggregatedProviderCourseDemandDetailsViewModel(
                    request.Ukprn,
                    request.CourseId,
                    request.Location,
                    request.Radius,
                    request.EmployerDemandIds);
                
                return View("FindApprenticeshipTrainingOpportunitiesForCourse", model);
            }
        }

        private async  Task<AggregatedProviderCourseDemandDetailsViewModel> BuildAggregatedProviderCourseDemandDetailsViewModel(
            int ukprn,
            int courseId,
            string location,
            string radius,
            IReadOnlyList<Guid> selectedEmployerDemandIds = null)
        {
            var result = await _mediator.Send(new GetProviderEmployerDemandDetailsQuery
            {
                Ukprn = ukprn,
                CourseId = courseId,
                Location = location,
                LocationRadius = radius
            });

            var model = (AggregatedProviderCourseDemandDetailsViewModel) result;
            model.SelectedEmployerDemandIds = selectedEmployerDemandIds ?? new List<Guid>();

            return model;
        }
    }
}