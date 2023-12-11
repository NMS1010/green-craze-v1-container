namespace green_craze_be_v1.Application.Intefaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;

        Task CreateTransaction();

        Task Commit();

        Task Rollback();

        Task<int> Save();
    }
}