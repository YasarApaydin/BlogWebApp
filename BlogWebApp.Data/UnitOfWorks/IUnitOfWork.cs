using BlogWebApp.Core.Entities;
using BlogWebApp.Data.Repositories.Concretes;
using System.Linq.Expressions;

namespace BlogWebApp.Data.UnitOfWorks
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        Repository<T> GetRepository<T>() where T : class, IEntityBase, new();
        Task<int> SaveAsync();
        int Save();

     
    }
}
