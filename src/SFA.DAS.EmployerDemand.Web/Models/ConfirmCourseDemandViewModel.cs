using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCachedCreateCourseDemand;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class ConfirmCourseDemandViewModel
    {
        public TrainingCourseViewModel TrainingCourse { get; set; }

        public static implicit operator ConfirmCourseDemandViewModel (GetCachedCreateCourseDemandQueryResult source)
        {
            return new ConfirmCourseDemandViewModel();
        }
    }
}