using Moq;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerDemand.Web.AcceptanceTests.Steps
{
    [Binding]
    public class ProviderInterestSteps
    {
        private readonly ScenarioContext _context;

        public ProviderInterestSteps (ScenarioContext context)
        {
            _context = context;
        }
        
        [Then("the Api is called with the course id (.*)")]
        public void WhenISearchForAField(string courseId)
        {
            var apiClient = _context.Get<Mock<IApiClient>>(ContextKeys.MockApiClient);
            
            apiClient.Verify(x =>
                x.Get<GetProviderEmployerDemandResponse>(It.Is<GetProviderEmployerDemandRequest>(request =>
                    request.GetUrl.Contains($"demand/aggregated/providers/{10000001}?courseId={courseId}")
                )), Times.Once);
            
        }
    }
}