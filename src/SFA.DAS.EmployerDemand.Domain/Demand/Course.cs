using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand
{
    public class Course
    {
        public int Id { get ; set ; }
        public string Title { get ; set ; }
        public int Level { get ; set ; }
        public string Route { get; set; }

        public static implicit operator Course(TrainingCourse source)
        {
            return new Course
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                Route = source.Route
            };
        }
    }
}