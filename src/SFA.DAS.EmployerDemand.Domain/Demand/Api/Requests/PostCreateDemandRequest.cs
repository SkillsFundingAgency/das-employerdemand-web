using System;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;
using SFA.DAS.EmployerDemand.Domain.Locations.Api.Responses;

namespace SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests
{
    public class PostCreateDemandRequest : IPostApiRequest<PostCreateDemandData>
    {
        public PostCreateDemandData Data { get; set; }

        public PostCreateDemandRequest (PostCreateDemandData data)
        {
            Data = data;
        }

        public string PostUrl => "demand/create";
    }
    
    public class PostCreateDemandData
    {
        public Guid Id { get ; set ; }
        public LocationItem Location { get; set; }
        public TrainingCourse TrainingCourse { get ; set ; }
        public int NumberOfApprentices { get ; set ; }
        public string OrganisationName { get ; set ; }
        public string ContactEmail { get ; set ; }
    }

    public class PostCreateDemandResponse
    {
        public Guid Id { get ; set ; }
    }
}