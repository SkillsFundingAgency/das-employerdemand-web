using System;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class CompletedCourseDemandViewModel : CourseDemandViewModelBase
    {
        public Guid Id { get; set; }
        public TrainingCourseViewModel TrainingCourse { get; set; }
        public string ContactEmailAddress { get ; set ; }
        public string FindApprenticeshipTrainingCourseUrl { get ; set ; }
        public string NumberOfApprentices { get ; set ; }

        public static implicit operator CompletedCourseDemandViewModel (VerifiedCourseDemand source)
        {
            if (source == null)
            {
                return null;
            }
            
            return new CompletedCourseDemandViewModel
            {
                Id = source.Id,
                TrainingCourse = source.Course,
                Location = source.Location.Name,
                ContactEmailAddress = source.ContactEmail,
                NumberOfApprentices = source.NumberOfApprentices != 0 ? source.NumberOfApprentices.ToString() : ""
            };
        }
    }
}