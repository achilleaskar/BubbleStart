using BubbleStart.Messages;
using BubbleStart.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BubbleStart.Database
{
    public class GenericRepository : IDisposable
    {
        protected readonly MainDatabase Context;

        public GenericRepository()
        {
            this.Context = new MainDatabase();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindUserAsync(string userName)
        {
            return await Context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : BaseModel
        {
            if (filter == null)
            {
                return await Context.Set<TEntity>().ToListAsync();
            }
            else
            {
                return await Context.Set<TEntity>().Where(filter).ToListAsync();
            }
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Add<TEntity>(TEntity model) where TEntity : BaseModel
        {
            Context.Set<TEntity>().Add(model);
        }

        public virtual void Delete<TEntity>(TEntity entity)
             where TEntity : BaseModel
        {
            var dbSet = Context.Set<TEntity>();
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsyncSortedByUserName()
        {
            return await Context.Set<User>().OrderBy(x => x.UserName).ToListAsync();
        }

        public async Task<IEnumerable<Customer>> LoadAllCustomersAsync()
        {
            try
            {
                return await Context.Set<Customer>()
                        .Include(c => c.Illness)
                        .Include(c => c.WeightHistory)
                        .Include(c => c.ShowUps)
                        .OrderBy(x => x.SureName).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
               
            }
            finally
            { }
        }
    }
}