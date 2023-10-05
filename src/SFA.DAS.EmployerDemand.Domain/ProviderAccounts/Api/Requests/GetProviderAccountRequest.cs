using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.ProviderAccounts.Api.Requests
{
    public class GetProviderAccountRequest : IGetApiRequest
    {
        private readonly int _ukprn;

        public GetProviderAccountRequest(int ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"provideraccounts/{_ukprn}";
    }
}