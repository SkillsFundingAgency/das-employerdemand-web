using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerDemand.Web.Infrastructure;

namespace SFA.DAS.EmployerDemand.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
        [Route("start", Name = RouteNames.ServiceStart, Order = 1)]
        public IActionResult Index()
        {
            return View();
        }
    }
}