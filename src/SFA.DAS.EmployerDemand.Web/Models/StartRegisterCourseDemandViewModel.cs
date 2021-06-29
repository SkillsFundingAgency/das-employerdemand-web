using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class StartRegisterCourseDemandViewModel
    {
        public TrainingCourseViewModel TrainingCourse { get; set; }
        public short? EntryPoint { get ; set ; }

        public static implicit operator StartRegisterCourseDemandViewModel(GetStartCourseDemandQueryResult source)
        {
            return new StartRegisterCourseDemandViewModel
            {
                TrainingCourse = source.Course,
                EntryPoint = source.EntryPoint
            };
        }
    }
}