using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Specification;
using green_craze_be_v1.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace green_craze_be_v1.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext _dbContext;
        private readonly DbSet<T> _entities;

        public GenericRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
            _entities = dbContext.Set<T>();
        }

        public bool Delete(T entity)
        {
            if (entity == null)
            {
                throw new NotFoundException("Cannot find this entity");
            }
            try
            {
                var x = _entities.Remove(entity);
                return x.State == EntityState.Deleted;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot delete this entity", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await _entities.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot get entity list", ex);
            }
        }

        public async Task<T> GetById(object id)
        {
            try
            {
                return await _entities.FindAsync(id) ?? throw new NotFoundException("Cannot find this entity");
            }
            catch (Exception ex)
            {
                throw new NotFoundException("Cannot find this entity", ex);
            }
        }

        public async Task<bool> Insert(T entity)
        {
            if (entity == null)
            {
                throw new NotFoundException("Cannot find this entity");
            }
            try
            {
                var x = await _entities.AddAsync(entity);
                return x.State == EntityState.Added;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot insert this entity", ex);
            }
        }

        public bool Update(T entity)
        {
            if (entity == null)
            {
                throw new NotFoundException("Cannot find this entity");
            }
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot update this entity", ex);
            }
        }

        // Specification Pattern
        public async Task<List<T>> ListAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specifications)
        {
            return await ApplySpecification(specifications).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specifications)
        {
            return SpecificationEvaluator<T>.GetQuery(_entities.AsQueryable(), specifications);
        }
    }
}