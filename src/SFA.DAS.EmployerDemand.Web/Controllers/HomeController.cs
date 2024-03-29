using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateProviderInterest;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.UpdateCachedProviderInterest;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedProviderInterest;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetProviderEmployerDemandDetails;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Web.Infrastructure;
using SFA.DAS.EmployerDemand.Web.Infrastructure.Authorization;
using SFA.DAS.EmployerDemand.Web.Models;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.Provider.Shared.UI.Models;
using EmployerDemands = SFA.DAS.EmployerDemand.Domain.Demand.EmployerDemands;

namespace SFA.DAS.EmployerDemand.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.EmployerDemand)]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Domain.Configuration.EmployerDemand _demandConfig;
        private readonly ProviderSharedUIConfiguration _config;

        public HomeController (
            IMediator mediator, 
            IOptions<ProviderSharedUIConfiguration> config, 
            IOptions<Domain.Configuration.EmployerDemand> demandConfig)
        {
            _mediator = mediator;
            _demandConfig = demandConfig.Value;
            _config = config.Value;
        }
        
        [Route("", Name = RouteNames.ProviderServiceStartDefault, Order = 0)]
        [Route("start", Name = RouteNames.ProviderServiceStart, Order = 1)]
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
                SelectedRoutes = request.Routes
            });

            var model = (AggregatedProviderCourseDemandViewModel) result;
            ViewData["ProviderDashboard"] = _config.DashboardUrl + "account";
            return View(model);
        }

        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}", Name = RouteNames.ProviderDemandDetails)]
        public async Task<IActionResult> FindApprenticeshipTrainingOpportunitiesForCourse(
            int ukprn, 
            int courseId, 
            [FromQuery]string location, 
            [FromQuery]string radius,
            Guid? id = null,
            bool fromLocation = false)
        {
            var model = await BuildAggregatedProviderCourseDemandDetailsViewModel(
                ukprn,
                courseId,
                location,
                radius,
                (id.HasValue && id.Value == Guid.Empty) ? null : id,
                fromLocation);

            return View(model);
        }

        [HttpPost]
        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}", Name = RouteNames.PostProviderDemandDetails)]
        public async Task<IActionResult> PostFindApprenticeshipTrainingOpportunitiesForCourse(ProviderRegisterInterestRequest request)
        {
            try
            {
                var result = await _mediator.Send(new CreateCachedProviderInterestCommand
                {
                    Ukprn = request.Ukprn,
                    Id = request.Id ?? Guid.NewGuid(),
                    ProviderOffersThisCourse = request.ProviderOffersThisCourse,
                    ProviderName = User.Identity.Name,
                    Website = request.ProviderWebsite,
                    EmailAddress = request.ProviderEmail,
                    PhoneNumber = request.ProviderTelephoneNumber,
                    EmployerDemands = BuildEmployerDemands(request.EmployerDemands),
                    Course = new Course
                    {
                        Id = request.CourseId,
                        Level = request.CourseLevel,
                        Route = request.CourseSector,
                        Title = request.CourseTitle
                    }
                });

                var routeName = RouteNames.ConfirmProviderDetails;
                if (string.IsNullOrEmpty(request.ProviderEmail) ||
                    string.IsNullOrEmpty(request.ProviderTelephoneNumber))
                {
                    routeName = RouteNames.EditProviderDetails;
                }
                
                return RedirectToRoute(routeName, new {
                    id = result.Id, 
                    ukprn = request.Ukprn,
                    courseId = request.CourseId
                });
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
                    request.Id, false, true);
                
                return View("FindApprenticeshipTrainingOpportunitiesForCourse", model);
            }
        }

        [HttpGet]
        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}/confirm/{id}", Name = RouteNames.ConfirmProviderDetails)]
        public async Task<IActionResult> ConfirmProviderDetails(int ukprn, int courseId, Guid id)
        {
            var result = await _mediator.Send(new GetCachedProviderInterestQuery
            {
                Id = id
            });

            if (result.ProviderInterest == null)
            {
                return RedirectToRoute(RouteNames.ProviderDemandDetails, new {ukprn, courseId});
            }
            
            var model = (ProviderContactDetailsViewModel)result.ProviderInterest; 
            
            return View(model);
        }
        
        [HttpGet]
        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}/edit/{id}", Name = RouteNames.EditProviderDetails)]
        public async Task<IActionResult> EditProviderDetails(int ukprn, int courseId, Guid id)
        {
            var result = await _mediator.Send(new GetCachedProviderInterestQuery
            {
                Id = id
            });
            
            if (result.ProviderInterest == null)
            {
                return RedirectToRoute(RouteNames.ProviderDemandDetails, new {ukprn, courseId});
            }
            
            var model = (ProviderContactDetailsViewModel)result.ProviderInterest; 
            
            return View(model);
        }
        
        [HttpPost]
        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}/edit/{id}", Name = RouteNames.PostEditProviderDetails)]
        public async Task<IActionResult> PostEditProviderDetails(UpdateProviderInterestDetails request)
        {
            try
            {
                var result = await _mediator.Send(new UpdateCachedProviderInterestCommand
                {
                    Id = request.Id,
                    Website = request.Website,
                    EmailAddress = request.EmailAddress,
                    PhoneNumber = request.PhoneNumber
                });
            
                if (result.Id == null)
                {
                    return RedirectToRoute(RouteNames.ProviderDemandDetails, new {request.Ukprn, request.CourseId});
                }
                
                return RedirectToRoute(RouteNames.ConfirmProviderDetails, new
                {
                    id = result.Id, ukprn = request.Ukprn, courseId = request.CourseId
                });
            }
            catch (ValidationException e)
            {
                foreach (var member in e.ValidationResult.MemberNames)
                {
                    ModelState.AddModelError(member.Split('|')[0], member.Split('|')[1]);
                }

                var result = await _mediator.Send(new GetCachedProviderInterestQuery
                {
                    Id = request.Id
                });

                var model = (ProviderContactDetailsViewModel)result.ProviderInterest;
                model.Website = request.Website;
                model.EmailAddress = request.EmailAddress;
                model.PhoneNumber = request.PhoneNumber;
                
                return View("EditProviderDetails", model);
            }
        }

        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}/confirm/{id}/review", Name = RouteNames.ReviewProviderDetails)]
        public async Task<IActionResult> ReviewProviderDetails(int ukprn, int courseId, Guid id)
        {
            var result = await _mediator.Send(new GetCachedProviderInterestQuery
            {
                Id = id
            });

            if (result.ProviderInterest == null)
            {
                return RedirectToRoute(RouteNames.ProviderDemandDetails, new { ukprn, courseId });
            }

            var model = (ReviewProviderDetailsViewModel) result.ProviderInterest;

            model.FindApprenticeshipTrainingCourseUrl =$"{_demandConfig.FindApprenticeshipTrainingUrl}/courses/{courseId}/providers/{ukprn}";

            return View(model);
        }

        [HttpPost]
        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}/shared-contact-details/{id}", Name = RouteNames.PostCreateProviderInterest)]
        public async Task<IActionResult> PostCreateProviderInterest(CreateProviderInterestRequest request)
        {
            await _mediator.Send((CreateProviderInterestCommand)request);

            return RedirectToRoute(
                RouteNames.CreateProviderInterestCompleted, 
                new
                {
                    request.Ukprn,
                    request.CourseId,
                    request.Id
                });
        }

        [Route("{ukprn}/find-apprenticeship-opportunities/{courseId}/shared-contact-details/{id}", Name = RouteNames.CreateProviderInterestCompleted)]
        public async Task<IActionResult> CreateProviderInterestCompleted(int ukprn, int courseId, Guid id)
        {
            var result = await _mediator.Send(new GetCachedProviderInterestQuery
            {
                Id = id
            });

            if (result.ProviderInterest == null)
            {
                return RedirectToRoute(RouteNames.ProviderDemandDetails, new { ukprn, courseId });
            }

            var model = (CreateProviderInterestCompletedViewModel) result.ProviderInterest;
            model.FindApprenticeshipTrainingUrl = _demandConfig.FindApprenticeshipTrainingUrl;

            return View(model);
        }

        [Route("dashboard", Name = RouteNames.Dashboard)]
        public IActionResult Dashboard()
        {
            return RedirectPermanent(_config.DashboardUrl);
        }

        private async  Task<AggregatedProviderCourseDemandDetailsViewModel> BuildAggregatedProviderCourseDemandDetailsViewModel(
            int ukprn,
            int courseId,
            string location,
            string radius,
            Guid? cachedObjectId = null,
            bool fromLocation = false,
            bool validationException = false)
        {
            var result = await _mediator.Send(new GetProviderEmployerDemandDetailsQuery
            {
                Ukprn = ukprn,
                CourseId = courseId,
                Location = location,
                LocationRadius = radius,
                Id = cachedObjectId,
                FromLocation = fromLocation
            });

            var model = (AggregatedProviderCourseDemandDetailsViewModel) result;

            if (validationException)
            {
                model.SelectedEmployerDemandIds = new List<Guid>();
            }
            
            return model;
        }

        private IEnumerable<EmployerDemands> BuildEmployerDemands(IReadOnlyCollection<string> source)
        {
            var returnList = new List<EmployerDemands>();
            if (source == null || source.Count == 0)
            {
                return returnList;
            }
            foreach (var employerDemand in source)
            {
                var demand = employerDemand.Split('|');
                returnList.Add(new EmployerDemands
                {
                    EmployerDemandId = Guid.Parse(demand[0]),
                    NumberOfApprentices = int.Parse(demand[1]),
                    LocationName = demand[2]
                });
            }

            return returnList;
        }
    }
}