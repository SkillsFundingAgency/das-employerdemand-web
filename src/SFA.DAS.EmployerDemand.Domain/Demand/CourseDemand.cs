using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class CourseDemand
    {
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }
        public Course Course { get; set; }

        public static implicit operator CourseDemand(ProviderEmployerDemandItem source)
        {
            return new CourseDemand
            {
                Course = source.TrainingCourse,
                NumberOfApprentices = source.Apprentices,
                NumberOfEmployers = source.Employers
            };
        }
    }
}