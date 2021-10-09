using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ModelsCore.Enums;
using SportPlannerApi.DataLayer.Specifications;
using SportPlannerIngestion.DataLayer.Data;
using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportPlannerApi.DataLayer.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly SportPlannerContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<T> _entities;

        public Repository(SportPlannerContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entities = _context.Set<T>();
        }

        public async Task<IEnumerable<Tdto>> GetAll<Tdto>(int limit = 100)
        {
            var entities = await _entities.Take(limit).ToListAsync();

            return _mapper.Map<IEnumerable<Tdto>>(entities);
        }

        public async Task<IEnumerable<Tdto>> Get<Tdto>(ISpecification<T> spec, int limit = 100)
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_entities.AsQueryable(),
                    (current, include) => current.Include(include));

            var entities = await queryableResultWithIncludes
                .Where(spec.Filter)
                .Take(limit)
                .ToListAsync();

            return _mapper.Map<IEnumerable<Tdto>>(entities);
        }

        public async Task<(CrudResult, Tdto)> Add<Tdto>(Tdto entityDto)
        {
            var entity = _mapper.Map<T>(entityDto);
            await _entities.AddAsync(entity);

            var rowsAdded = _context.SaveChanges();
            if (rowsAdded == 0)
            {
                return (CrudResult.NoAction, default);
            }

            return (CrudResult.Ok, entityDto);
        }

        public async Task<CrudResult> Update<Tdto>(GetByIdSpecification<T> spec, Tdto entityDto)
        {
            var trackedEntity = (await Get<T>(spec)).SingleOrDefault();
            if (trackedEntity is null)
            {
                return CrudResult.NotFound;
            }

            _mapper.Map(entityDto, trackedEntity);
            _context.SaveChanges();
            return CrudResult.Ok;
        }

        public async Task<CrudResult> Delete(Guid id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity is null)
            {
                return CrudResult.NotFound;
            }

            _entities.Remove(entity);

            return CrudResult.Ok;
        }
    }
}
