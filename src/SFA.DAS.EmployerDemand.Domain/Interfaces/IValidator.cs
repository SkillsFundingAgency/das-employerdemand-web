using System.Threading.Tasks;
using SFA.DAS.EmployerDemand.Domain.Validation;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IValidator<in T>
    {
        Task<ValidationResult> ValidateAsync(T item);
    }
}