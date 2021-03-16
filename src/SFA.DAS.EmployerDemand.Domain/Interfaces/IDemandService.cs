using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IDemandService
    {
        Task<TrainingCourse> GetCreateCourseDemand(int trainingCourseId);
        Task CreateCacheCourseDemand(ICourseDemand item);
    }
}