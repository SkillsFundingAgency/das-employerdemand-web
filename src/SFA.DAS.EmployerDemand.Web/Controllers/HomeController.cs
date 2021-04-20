using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
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
        public async Task<IActionResult> FindApprenticeshipTrainingOpportunities(FindApprenticeshipTrainingOpportunitiesRequest request)
        {
            var result = await _mediator.Send(new GetProviderEmployerDemandQuery
            {
                Ukprn = request.Ukprn,
                CourseId = request.SelectedCourseId,
                Location = request.Location,
                LocationRadius = request.Radius,
                Sectors = request.Sectors
            });

            var locationList = AggregatedProviderCourseDemandViewModel.BuildLocationRadiusList();
            var model = new AggregatedProviderCourseDemandViewModel
            {
                SelectedCourseId = result.SelectedCourseId,
                SelectedCourse = result.SelectedCourseId != null ? result.Courses.Select(c => (TrainingCourseViewModel)c).ToList().SingleOrDefault(c => c.Id.Equals(result.SelectedCourseId))?.TitleAndLevel : "",
                TotalFiltered = result.TotalFiltered,
                TotalResults = result.TotalResults,
                Courses = result.Courses.Select(c => (TrainingCourseViewModel)c).ToList(),
                CourseDemands = result.CourseDemands.Select(c => (ProviderCourseDemandViewModel)c),
                SelectedLocation = result.SelectedLocation,
                Location = result.SelectedLocation?.Name,
                SelectedRadius = result.SelectedRadius != null && locationList.ContainsKey(result.SelectedRadius) ? result.SelectedRadius : locationList.First().Key,
                SelectedSectors = result.SelectedSectors.Any() ? result.SelectedSectors.Select(c => c.Route).ToList() : new List<string>(),
                Sectors = result.SelectedSectors.Select(sector => new SectorViewModel(sector, request.Sectors)).ToList(),
            };
            
            ViewData["ProviderDashboard"] = _config.ProviderPortalUrl;
            return View(model);
        }
    }
}