using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerDemand.Domain.Interfaces
{
    public interface ICacheStorageService
    {
        Task<T> RetrieveFromCache<T>(string key);
        Task SaveToCache<T>(string key, T item, TimeSpan expiryTimeFromNow);
        Task DeleteFromCache(string key);
    }
}