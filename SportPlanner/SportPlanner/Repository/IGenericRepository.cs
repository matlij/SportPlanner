﻿using System.Threading.Tasks;

namespace SportPlanner.Repository
{
    public interface IGenericRepository
    {
        Task<bool> DeleteAsync(string requestUri);

        Task<T> GetAsync<T>(string requestUri);

        Task<bool> PatchAsync<T>(string requestUri, T body);

        Task<T> PostAsync<T>(string requestUri, T body);

        Task<bool> PutAsync<T>(string requestUri, T body);
    }
}