using System;
using SFA.DAS.EmployerDemand.Domain.Demand;

namespace SFA.DAS.EmployerDemand.Web.Models
{ 
    public class RegisterCourseDemandViewModel : CourseDemandViewModelBase
    {
        public Guid? CreateDemandId { get ; set ; }
        public bool? NumberOfApprenticesKnown { get; set; }
        public TrainingCourseViewModel TrainingCourse { get; set; }

        public string OrganisationName { get; set; }
        public string ContactEmailAddress { get; set; }
        public string NumberOfApprentices { get; set; }
        public Guid? ExpiredCourseDemandId { get ; set ; }
        public short? EntryPoint { get ; set ; }

        public static implicit operator RegisterCourseDemandViewModel(RegisterDemandRequest request)
        {
            return new RegisterCourseDemandViewModel
            {
                CreateDemandId = request.CreateDemandId,
                OrganisationName = request.OrganisationName,
                Location = request.Location,
                ContactEmailAddress = request.ContactEmailAddress,
                NumberOfApprentices = request.NumberOfApprentices,
                NumberOfApprenticesKnown = request.NumberOfApprenticesKnown,
                ExpiredCourseDemandId = null,
                EntryPoint = request.EntryPoint
            };
        }

        public static implicit operator RegisterCourseDemandViewModel(CourseDemandRequest queryResult)
        {
            return new RegisterCourseDemandViewModel
            {
                CreateDemandId = queryResult.Id,
                OrganisationName = queryResult.OrganisationName,
                Location = queryResult.LocationItem?.Name,
                ContactEmailAddress = queryResult.ContactEmailAddress,
                NumberOfApprentices = queryResult.NumberOfApprentices,
                NumberOfApprenticesKnown = queryResult.NumberOfApprenticesKnown,
                TrainingCourse = queryResult.Course,
                ExpiredCourseDemandId = queryResult.ExpiredCourseDemandId,
                EntryPoint = queryResult.EntryPoint
            };
        }
    }
}