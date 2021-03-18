using System;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class PostCreateDemandData : ICourseDemand
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public int TrainingCourseId { get; set; }
        public string OrganisationName { get; set; }
        public string NumberOfApprentices { get; set; }
        [JsonIgnore]
        public string Location { get; set; }
        public string ContactEmailAddress { get; set; }
        [JsonIgnore]
        public Location LocationItem { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        [JsonIgnore]
        public bool? NumberOfApprenticesKnown { get; set; }

        public LocationItem CourseDemandLocation => BuildCourseLocation();

        public TrainingCourse TrainingCourse => BuildTrainingCourse();

        private LocationItem BuildCourseLocation()
        {
            return new LocationItem
            {
                Name = LocationItem.Name,
                LocationPoint = new LocationPoint
                {
                    GeoPoint = LocationItem.LocationPoint
                }
            };
        }

        private TrainingCourse BuildTrainingCourse()
        {
            return new TrainingCourse
            {
                Id = Course.Id,
                Level = Course.Level,
                Title = Course.Title
            };
        }
    }
}