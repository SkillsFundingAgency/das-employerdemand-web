namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class ProviderContactDetails
    {
        public int Ukprn { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string Website { get; set; }

        public static implicit operator ProviderContactDetails(Api.Responses.ProviderContactDetails source)
        {
            return new ProviderContactDetails
            {
                Ukprn = source.Ukprn,
                Website = source.Website,
                EmailAddress = source.EmailAddress,
                TelephoneNumber = source.TelephoneNumber
            };
        }
    }
}