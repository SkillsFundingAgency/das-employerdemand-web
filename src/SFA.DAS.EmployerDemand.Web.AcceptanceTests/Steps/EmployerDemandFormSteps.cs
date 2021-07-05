using System;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerDemand.Web.AcceptanceTests.Steps
{
    [Binding]
    public class EmployerDemandFormSteps
    {
        private readonly ScenarioContext _context;

        public EmployerDemandFormSteps(ScenarioContext context)
        {
            _context = context;
        }

    }
}

