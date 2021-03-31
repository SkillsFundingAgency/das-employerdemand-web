using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ProviderCourseDemandViewModel
    {
        public TrainingCourseViewModel Course { get ; set ; }
        public int NumberOfApprentices { get ; set ; }
        public int NumberOfEmployers { get ; set ; }

        public static implicit operator ProviderCourseDemandViewModel(ProviderCourseDemand source)
        {
            return new ProviderCourseDemandViewModel
            {
                NumberOfApprentices = source.NumberOfApprentices,
                NumberOfEmployers = source.NumberOfEmployers,
                Course = source.Course
            };
        }
    }
}