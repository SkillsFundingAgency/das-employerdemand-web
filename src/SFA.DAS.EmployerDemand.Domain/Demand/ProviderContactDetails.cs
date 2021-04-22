namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class ProviderContactDetails
    {
        public int Ukprn { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }

        public static implicit operator ProviderContactDetails(Api.Responses.ProviderContactDetails source)
        {
            if (source == null)
            {
                return null;
            }
            return new ProviderContactDetails
            {
                Ukprn = source.Ukprn,
                Website = source.Website,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber
            };
        }
    }
}