using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Services
{
    public class DemandService : IDemandService
    {
        private readonly IApiClient _apiClient;
        public DemandService (IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<TrainingCourse> GetCreateCourseDemand(int trainingCourseId)
        {
            var result =
                await _apiClient.Get<GetCreateCourseDemandResponse>(new GetCreateDemandRequest(trainingCourseId));

            return result?.Course;
        }
    }
}