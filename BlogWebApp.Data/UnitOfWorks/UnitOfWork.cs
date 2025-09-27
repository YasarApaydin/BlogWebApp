using BlogWebApp.Core.Entities;
using BlogWebApp.Data.Context;
using BlogWebApp.Data.Repositories.Concretes;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogWebApp.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext appDbContext;

        public UnitOfWork(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
            
        }
        public async ValueTask DisposeAsync()
        {
         await appDbContext.DisposeAsync();
        }

        public int Save()
        {
            return appDbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
          return  await appDbContext.SaveChangesAsync();
        }

      

        Repository<T> IUnitOfWork.GetRepository<T>()
        {
            return new Repository<T>(appDbContext);
        }

     

    }
}
