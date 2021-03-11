using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerDemand.Web.Models
{
    public class RegisterDemandRequest
    {
        public int TrainingCourseId { get; set; }
        public string OrganisationName { get; set; }
        public string ContactEmailAddress { get; set; }
        public int? NumberOfApprentices { get; set; }
        [Required(ErrorMessage = "Select yes if you know how many apprentices will take this apprenticeship training")]
        public bool? NumberOfApprenticesKnown { get; set; }
        [Required(ErrorMessage = "Enter a town, city or postcode")]
        public string Location { get; set; }
        
    }
}