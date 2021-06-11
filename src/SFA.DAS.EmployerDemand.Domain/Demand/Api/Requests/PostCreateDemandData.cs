using System;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class PostCreateDemandData
    {
        public PostCreateDemandData(ICourseDemand item)
        {
            Id = item.Id;
            OrganisationName = item.OrganisationName;
            ContactEmailAddress = item.ContactEmailAddress;
            Course = item.Course;
            Location = item.LocationItem;
            NumberOfApprentices = item.NumberOfApprenticesKnown.HasValue && item.NumberOfApprenticesKnown.Value 
                ? Convert.ToInt32(item.NumberOfApprentices) : 0;
        }

        public Guid Id { get; set; }
        public string OrganisationName { get; set; }
        public int NumberOfApprentices { get; set; }
        public string ContactEmailAddress { get; set; }
        [JsonIgnore]
        public Location Location { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        
        public LocationItem LocationItem => BuildCourseLocation();

        public TrainingCourse TrainingCourse => BuildTrainingCourse();
        
        public string ResponseUrl { get ; set ; }
        public string StopSharingUrl { get ; set ; }

        private LocationItem BuildCourseLocation()
        {
            return new LocationItem
            {
                Name = Location.Name,
                LocationPoint = new LocationPoint
                {
                    GeoPoint = Location.LocationPoint
                }
            };
        }

        private TrainingCourse BuildTrainingCourse()
        {
            return new TrainingCourse
            {
                Id = Course.Id,
                Level = Course.Level,
                Title = Course.Title,
                Route = Course.Route
            };
        }
    }
}