using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.UpdateCachedProviderInterest
{
    public class UpdateCachedProviderInterestCommandHandler : IRequestHandler<UpdateCachedProviderInterestCommand, UpdateCachedProviderInterestCommandResult>
    {
        private readonly IValidator<UpdateCachedProviderInterestCommand> _validator;
        private readonly IDemandService _service;

        public UpdateCachedProviderInterestCommandHandler (IValidator<UpdateCachedProviderInterestCommand> validator, IDemandService service)
        {
            _validator = validator;
            _service = service;
        }
        public async Task<UpdateCachedProviderInterestCommandResult> Handle(UpdateCachedProviderInterestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }

            var cachedInterest = await _service.GetCachedProviderInterest(request.Id);

            if (cachedInterest == null)
            {
                return new UpdateCachedProviderInterestCommandResult
                {
                    Id = null
                };
            }
            
            cachedInterest.Website = request.Website;
            cachedInterest.PhoneNumber = request.PhoneNumber;
            cachedInterest.EmailAddress = request.EmailAddress;

            await _service.CreateCachedProviderInterest(cachedInterest);
            
            return new UpdateCachedProviderInterestCommandResult
            {
                Id = request.Id
            };
        }
    }
}