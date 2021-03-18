using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class TrainingCourseViewModel
    {
        public int Id { get ; private set ; }
        public string Title { get ; set ; }
        public string TitleAndLevel { get; private set; }
        public int Level { get ; set; }
        public static implicit operator TrainingCourseViewModel(Course course)
        {
            if (course == null)
            {
                return null;
            }
            
            return new TrainingCourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                TitleAndLevel = $"{course.Title} (level {course.Level})",
                Level = course.Level
            };
        }
    }
}