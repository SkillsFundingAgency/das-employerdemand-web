using System;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ConfirmCourseDemandViewModel : CourseDemandViewModelBase
    {
        public Guid Id { get; set; }
        public TrainingCourseViewModel TrainingCourse { get; set; }
        public bool NumberOfApprenticesKnown { get ; set ; }
        public string NumberOfApprentices { get ; set ; }
        public string OrganisationName { get ; set ; }
        public string ContactEmailAddress { get ; set ; }

        public static implicit operator ConfirmCourseDemandViewModel (CourseDemandRequest source)
        {
            if (source == null)
            {
                return null;
            }
            
            return new ConfirmCourseDemandViewModel
            {
                Id = source.Id,
                TrainingCourse = source.Course,
                Location = source.LocationItem.Name,
                OrganisationName = source.OrganisationName,
                ContactEmailAddress = source.ContactEmailAddress,
                NumberOfApprentices = source.NumberOfApprentices,
                NumberOfApprenticesKnown = source.NumberOfApprenticesKnown ?? false
            };
        }
    }
}