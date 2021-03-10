namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class RegisterCourseDemandViewModel
    {
        public TrainingCourseViewModel TrainingCourse { get; set; }
        
        public string OrganisationName { get; set; }
        public string ContactEmailAddress { get; set; }
        public int? NumberOfApprentices { get; set; }
        public string LocationName { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}