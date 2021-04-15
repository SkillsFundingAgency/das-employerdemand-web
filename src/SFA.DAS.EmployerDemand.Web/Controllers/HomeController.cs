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

            var model = (AggregatedProviderCourseDemandViewModel) result;
            ViewData["ProviderDashboard"] = _config.ProviderPortalUrl;
            return View(model);
        }
    }
}