using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ConfirmCourseDemandViewModel
    {
        public TrainingCourseViewModel TrainingCourse { get; set; }
        public string LocationName { get ; set ; }
        public bool NumberOfApprenticesKnown { get ; set ; }
        public string NumberOfApprentices { get ; set ; }
        public string OrganisationName { get ; set ; }
        public string ContactEmailAddress { get ; set ; }

        public static implicit operator ConfirmCourseDemandViewModel (CourseDemandRequest source)
        {
            return new ConfirmCourseDemandViewModel
            {
                TrainingCourse = source.Course,
                LocationName = source.LocationItem.Name,
                OrganisationName = source.OrganisationName,
                ContactEmailAddress = source.ContactEmailAddress,
                NumberOfApprentices = source.NumberOfApprentices,
                NumberOfApprenticesKnown = source.NumberOfApprenticesKnown ?? false
            };
        }
    }
}