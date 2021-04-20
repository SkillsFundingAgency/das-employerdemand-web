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

        public CreateCachedProviderInterestCommandHandler(IValidator<CreateCachedProviderInterestCommand> validator)
        {
            _validator = validator;
        }

        public async Task<CreateCachedProviderInterestResult> Handle(CreateCachedProviderInterestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult,null, null);
            }

            return new CreateCachedProviderInterestResult();
        }
    }
}