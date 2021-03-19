using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Responses;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand
{
    public class CreateCachedCourseDemandCommandHandler : IRequestHandler<CreateCachedCourseDemandCommand, CreateCachedCourseDemandCommandResult>
    {
        private readonly IDemandService _service;
        private readonly IValidator<CreateCachedCourseDemandCommand> _validator;

        public CreateCachedCourseDemandCommandHandler (IDemandService service, IValidator<CreateCachedCourseDemandCommand> validator)
        {
            _service = service;
            _validator = validator;
        }
        public async Task<CreateCachedCourseDemandCommandResult> Handle(CreateCachedCourseDemandCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            var result = new GetCreateCourseDemandResponse();

            if (!validationResult.ValidationDictionary.ContainsKey(nameof(request.Location)))
            {
                result = await _service.GetCreateCourseDemand(request.TrainingCourseId, request.Location);
                
                if (result.Location == null)
                {
                    validationResult.AddError(nameof(request.Location), "Enter a real town, city or postcode");
                }
            }
            
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }

            request.LocationItem = result.Location;
            request.Course = result.Course;
            
            await _service.CreateCacheCourseDemand(request);
            
            return new CreateCachedCourseDemandCommandResult
            {
                Id = request.Id
            };
        }
    }
}