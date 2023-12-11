using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace green_craze_be_v1.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;

        public AppDBContext Context { get; }

        public UnitOfWork(AppDBContext context)
        {
            Context = context;
        }

        private IDbContextTransaction _objTran;

        public IGenericRepository<T> Repository<T>() where T : class
        {
            _repositories ??= new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), Context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task Commit()
        {
            await _objTran.CommitAsync();
        }

        public async Task CreateTransaction()
        {
            _objTran = await Context.Database.BeginTransactionAsync();
        }

        public async Task Rollback()
        {
            await _objTran.RollbackAsync();
            await _objTran.DisposeAsync();
        }

        public async Task<int> Save()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}