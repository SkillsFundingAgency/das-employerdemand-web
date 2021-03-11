using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class RegisterDemandRequest
    {
        public int TrainingCourseId { get; set; }
        [Required(ErrorMessage = "Enter the name of the organisation")]
        public string OrganisationName { get; set; }
        [Required(ErrorMessage = "Enter an email address")]
        public string ContactEmailAddress { get; set; }
        public int? NumberOfApprentices { get; set; }
        [Required(ErrorMessage = "Enter a town, city or postcode")]
        public string Location { get; set; }
    }
}