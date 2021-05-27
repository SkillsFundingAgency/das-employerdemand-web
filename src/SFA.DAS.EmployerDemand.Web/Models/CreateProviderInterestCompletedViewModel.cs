using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class CreateProviderInterestCompletedViewModel
    {
        public int Ukprn { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public TrainingCourseViewModel Course { get; set; }
        public bool ProviderOffersThisCourse { get; set; }
        public string FindApprenticeshipTrainingUrl { get; set; }

        public static implicit operator CreateProviderInterestCompletedViewModel(ProviderInterestRequest source)
        {
            return new CreateProviderInterestCompletedViewModel
            {
                Ukprn = source.Ukprn,
                Course = source.Course,
                Website = source.Website,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber,
                ProviderOffersThisCourse = source.ProviderOffersThisCourse
            };
        }
    }
}