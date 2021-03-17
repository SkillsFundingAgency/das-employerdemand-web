using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand;

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

        public static implicit operator ConfirmCourseDemandViewModel (GetCachedCreateCourseDemandQueryResult source)
        {
            return new ConfirmCourseDemandViewModel
            {
                TrainingCourse = source.CourseDemand.Course,
                LocationName = source.CourseDemand.LocationItem.Name,
                OrganisationName = source.CourseDemand.OrganisationName,
                ContactEmailAddress = source.CourseDemand.ContactEmailAddress,
                NumberOfApprentices = source.CourseDemand.NumberOfApprentices,
                NumberOfApprenticesKnown = source.CourseDemand.NumberOfApprenticesKnown ?? false
            };
        }
    }
}