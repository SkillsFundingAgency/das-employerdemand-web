namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface IFatUrlBuilder
    {
        string BuildFatUrl(IProviderDemandInterest interest);
    }
}