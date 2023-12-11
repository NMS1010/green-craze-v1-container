using green_craze_be_v1.Application.Specification;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(object id);

        Task<bool> Insert(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        Task<T> GetEntityWithSpec(ISpecification<T> specification);

        Task<List<T>> ListAsync(ISpecification<T> specification);

        Task<int> CountAsync(ISpecification<T> specifications);
    }
}