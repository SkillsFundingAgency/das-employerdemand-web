using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class VerifyEmployerCourseDemandViewModel
    {
        public string EmailAddress { get; set; }
        public bool Verified { get ; set ; }

        public static implicit operator VerifyEmployerCourseDemandViewModel(CourseDemand source)
        {
            if (source == null)
            {
                return null;
            }
            return new VerifyEmployerCourseDemandViewModel
            {
                Verified = source.EmailVerified,
                EmailAddress = source.ContactEmailAddress
            };
        }
    }
}