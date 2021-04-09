using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class ProviderCourseDemand
    {
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }
        public Course Course { get; set; }

        public static implicit operator ProviderCourseDemand(ProviderEmployerDemandItem source)
        {
            return new ProviderCourseDemand
            {
                Course = source.TrainingCourse,
                NumberOfApprentices = source.Apprentices,
                NumberOfEmployers = source.Employers
            };
        }
    }
}