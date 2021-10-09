using ModelsCore.Enums;
using SportPlannerApi.DataLayer.Specifications;
using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportPlannerApi.DataLayer.DataAccess
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<Tdto>> GetAll<Tdto>(int limit = 100);

        Task<IEnumerable<Tdto>> Get<Tdto>(ISpecification<T> spec, int limit = 100);

        Task<(CrudResult result, Tdto dto)> Add<Tdto>(Tdto entityDto);

        //Task<CrudResult> Update<Tdto>(Guid id, Tdto entityDto);

        Task<CrudResult> Delete(Guid id);

        Task<CrudResult> Update<Tdto>(GetByIdSpecification<T> spec, Tdto entityDto);
    }
}
