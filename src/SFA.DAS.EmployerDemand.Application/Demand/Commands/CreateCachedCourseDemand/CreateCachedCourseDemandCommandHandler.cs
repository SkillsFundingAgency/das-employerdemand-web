using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedCourseDemand
{
    public class CreateCachedCourseDemandCommandHandler : IRequestHandler<CreateCourseDemandCommand, CreateCachedCourseDemandCommandResult>
    {
        private readonly IDemandService _service;
        private readonly IValidator<CreateCourseDemandCommand> _validator;

        public CreateCachedCourseDemandCommandHandler (IDemandService service, IValidator<CreateCourseDemandCommand> validator)
        {
            _service = service;
            _validator = validator;
        }
        public async Task<CreateCachedCourseDemandCommandResult> Handle(CreateCourseDemandCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }

            var result = await _service.GetCreateCourseDemand(request.TrainingCourseId, request.Location);

            if (result.Location == null)
            {
                validationResult.AddError(nameof(request.Location), "Enter a real town, city or postcode");
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