using ModelsCore.Enums;
using SportPlanner.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportPlanner.Repository.Interfaces
{
    public interface ILocalStorage<T>
    {
        Task<T> Get(Guid id);
        Task<CrudResult> Delete(Guid id);
        Task<CrudResult> Upsert(T entity);
        Task<List<User>> GetAll();
    }
}