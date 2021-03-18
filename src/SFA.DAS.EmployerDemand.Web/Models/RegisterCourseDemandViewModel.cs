using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class RegisterCourseDemandViewModel
    {
        public bool? NumberOfApprenticesKnown;
        public TrainingCourseViewModel TrainingCourse { get; set; }
        
        public string OrganisationName { get; set; }
        public string ContactEmailAddress { get; set; }
        public string NumberOfApprentices { get; set; }
        public string Location { get; set; }

        public static implicit operator RegisterCourseDemandViewModel(RegisterDemandRequest request)
        {
            return new RegisterCourseDemandViewModel
            {
                OrganisationName = request.OrganisationName,
                Location = request.Location,
                ContactEmailAddress = request.ContactEmailAddress,
                NumberOfApprentices = request.NumberOfApprentices,
                NumberOfApprenticesKnown = request.NumberOfApprenticesKnown
            };
        }

        public static implicit operator RegisterCourseDemandViewModel(CourseDemandRequest queryResult)
        {
            return new RegisterCourseDemandViewModel
            {
                OrganisationName = queryResult.OrganisationName,
                Location = queryResult.LocationItem?.Name,
                ContactEmailAddress = queryResult.ContactEmailAddress,
                NumberOfApprentices = queryResult.NumberOfApprentices,
                NumberOfApprenticesKnown = queryResult.NumberOfApprenticesKnown,
                TrainingCourse = queryResult.Course
            };
        }
    }
}