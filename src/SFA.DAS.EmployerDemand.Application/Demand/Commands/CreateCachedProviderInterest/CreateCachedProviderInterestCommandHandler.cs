using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CreateCachedProviderInterest
{
    public class CreateCachedProviderInterestCommandHandler : IRequestHandler<CreateCachedProviderInterestCommand, CreateCachedProviderInterestResult> 
    {
        private readonly IValidator<CreateCachedProviderInterestCommand> _validator;
        private readonly IDemandService _demandService;

        public CreateCachedProviderInterestCommandHandler(IValidator<CreateCachedProviderInterestCommand> validator, IDemandService demandService)
        {
            _validator = validator;
            _demandService = demandService;
        }

        public async Task<CreateCachedProviderInterestResult> Handle(CreateCachedProviderInterestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }

            var cachedInterest = await _demandService.GetCachedProviderInterest(request.Id);

            if (cachedInterest != null)
            {
                cachedInterest.EmployerDemands = request.EmployerDemands;

                await _demandService.CreateCachedProviderInterest(cachedInterest);

                return new CreateCachedProviderInterestResult
                {
                    Id = cachedInterest.Id
                };
            }

            await _demandService.CreateCachedProviderInterest(request);
            
            return new CreateCachedProviderInterestResult
            {
                Id = request.Id
            };
        }
    }
}