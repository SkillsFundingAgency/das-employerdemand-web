using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class GetProviderEmployerDemandRequest : IGetApiRequest
    {
        private readonly int _ukprn;
        private readonly int? _courseId;

        public GetProviderEmployerDemandRequest(int ukprn, int? courseId = null)
        {
            _ukprn = ukprn;
            _courseId = courseId;
        }

        public string GetUrl => $"providers/{_ukprn}/employer-demand?courseId={_courseId}";
    }
}