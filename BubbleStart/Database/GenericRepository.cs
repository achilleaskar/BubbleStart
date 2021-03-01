using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BubbleStart.Model;

namespace BubbleStart.Database
{
    public class GenericRepository : IDisposable
    {
        public MainDatabase Context;
        public DateTime Limit;
        protected readonly DateTime CloseLimit;


        public GenericRepository()
        {
            Context = new MainDatabase();
            // Context.Database.Log = Console.Write;

            if (DateTime.Today.Month > 7 && DateTime.Today.Day >= 20)
            {
                Limit = new DateTime(DateTime.Today.Year-1, 8, 20);

            }
            else
            {
                Limit = new DateTime(DateTime.Today.Year - 2, 8, 20);
            }
            CloseLimit = (DateTime.Today - Limit).TotalDays > 60 ? DateTime.Today.AddDays(-60) : Limit;
            //Context.Database.Log = Console.Write;
        }
        protected virtual void Dispose(bool b)
        {
            if (b)
            {
                Context.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        public async Task<User> FindUserAsync(string userName)
        {
            return await Context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }

        internal async Task<List<ShowUp>> GetAllShowUpsInRangeAsyncsAsync(DateTime StartDate, DateTime EndDate)
        {
            return await Context.ShowUps
           .Where(s => s.Arrived >= StartDate && s.Arrived < EndDate && s.Arrived >= Limit)
           .Include(s => s.Customer)
           .OrderBy(s => s.Arrived)
           .ToListAsync();
        }

        //public async Task<Payment> Gettest()
        //{
        //    return await Context.Payments.Where(p => p.Id == 67).FirstOrDefaultAsync();
        //}

        public async Task DeleteFromThis(Customer c, DateTime date)
        {
            var todel = await Context.Apointments.Where(p => p.Customer.Id == c.Id && p.DateTime >= date).ToListAsync();
            todel = todel.Where(a => a.DateTime.DayOfWeek == date.DayOfWeek && a.DateTime.TimeOfDay == date.TimeOfDay).ToList();
            Context.Apointments.RemoveRange(todel);
        }

        public async Task<List<Apointment>> GetApointmentsAsync(DateTime date)
        {
            DateTime tmpEndDate = date.AddDays(6);

            return await Context.Apointments.Where(a => a.DateTime >= date && a.DateTime < tmpEndDate && a.DateTime >= Limit)
                .Include(a => a.Customer)
                .Include(a => a.Customer.ShowUps)
                .ToListAsync();
        }

        //public TEntity GetById<TEntity>(int id) where TEntity : BaseModel
        //{
        //    return Context.Set<TEntity>().Where(p => p.Id == id).FirstOrDefault();
        //}

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : BaseModel
        {
            if (filter == null)
            {
                return await Context.Set<TEntity>().ToListAsync();
            }

            return await Context.Set<TEntity>().Where(filter).ToListAsync();
        }



        public async Task<Customer> GetFullCustomerById(int id)
        {
            try
            {
                return await Context.Set<Customer>().Where(c => c.Id == id)
                        .Include(f => f.Programs.Select(t => t.Payments))
                        .Include(g => g.Payments)
                        .Include(d => d.WeightHistory)
                        .Include(c => c.Illness)
                        .Include(e => e.ShowUps)
                        .Include(h => h.Changes)
                        .Include(a => a.Apointments)
                        .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool HasChanges()
        {
            //var AddedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            //AddedEntities.ForEach(E =>
            //{
            //    if (E.CurrentValues.PropertyNames.Contains("CreatedDate"))
            //    {
            //        E.Property("CreatedDate").CurrentValue = DateTime.Now;
            //    }
            //});

            //var EditedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            //EditedEntities.ForEach(E =>
            //{
            //    if (E.OriginalValues.PropertyNames.Contains("ModifiedDate"))
            //    {
            //        //   E.Property("ModifiedDate").CurrentValue = DateTime.Now;
            //    }
            //});

            //var changes = from e in Context.ChangeTracker.Entries()
            //              where e.State.HasFlag(EntityState.Added) ||
            //                  e.State.HasFlag(EntityState.Modified) ||
            //                  e.State.HasFlag(EntityState.Deleted)
            //              select e;

            //foreach (var change in changes)
            //{
            //    if (change.State == EntityState.Added)
            //    {
            //        // Log Added
            //    }
            //    else if (change.State == EntityState.Modified)
            //    {
            //        // Log Modified
            //        var item = change.Entity;
            //        var originalValues = Context.Entry(item).OriginalValues;
            //        var currentValues = Context.Entry(item).CurrentValues;

            //        foreach (string propertyName in originalValues.PropertyNames)
            //        {
            //            var original = originalValues[propertyName];
            //            var current = currentValues[propertyName];

            //            if (original != current)
            //            {
            //            }
            //            Console.WriteLine("Property {0} changed from {1} to {2}",
            //         propertyName,
            //         originalValues[propertyName],
            //         currentValues[propertyName]);
            //        }
            //    }
            //    else if (change.State == EntityState.Deleted)
            //    {
            //        // log deleted
            //    }
            //}
            ////return Context.ChangeTracker.HasChanges();
            IEnumerable<DbEntityEntry> res = from e in Context.ChangeTracker.Entries()
                                             where
                                             e.State.HasFlag(EntityState.Added) ||
                                             e.State.HasFlag(EntityState.Modified) ||
                                             e.State.HasFlag(EntityState.Deleted)
                                             select e;
            return Context.ChangeTracker.HasChanges();
            return res.Any();
        }

        public async Task SaveAsync()
        {

            List<DbEntityEntry> AddedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                if (E.CurrentValues.PropertyNames.Contains("CreatedDate"))
                {
                    E.Property("CreatedDate").CurrentValue = DateTime.Now;
                }
            });

            List<DbEntityEntry> EditedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                if (E.OriginalValues.PropertyNames.Contains("ModifiedDate"))
                {
                    //   E.Property("ModifiedDate").CurrentValue = DateTime.Now;
                }
            });

            // ReSharper disable once UnusedVariable
            IEnumerable<DbEntityEntry> changes = from e in Context.ChangeTracker.Entries()
                                                 where e.State != EntityState.Unchanged
                                                 select e;

            //foreach (var change in changes)
            //{
            //    if (change.State == EntityState.Added)
            //    {
            //        // Log Added
            //    }
            //    else if (change.State == EntityState.Modified)
            //    {
            //        // Log Modified
            //        var item = change.Entity;
            //        var originalValues = Context.Entry(item).OriginalValues;
            //        var currentValues = Context.Entry(item).CurrentValues;

            //        foreach (string propertyName in originalValues.PropertyNames)
            //        {
            //            var original = originalValues[propertyName];
            //            var current = currentValues[propertyName];

            //            Console.WriteLine("Property {0} changed from {1} to {2}",
            //         propertyName,
            //         originalValues[propertyName],
            //         currentValues[propertyName]);
            //        }
            //    }
            //    else if (change.State == EntityState.Deleted)
            //    {
            //        // log deleted
            //    }
            //}
            await Context.SaveChangesAsync();
        }

        public void RollBack()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            RejectNavigationChanges();
        }

        private void RejectNavigationChanges()
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;
            var deletedRelationships = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).Where(e => e.IsRelationship && !RelationshipContainsKeyEntry(e));
            var addedRelationships = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(e => e.IsRelationship);

            foreach (var relationship in addedRelationships)
                relationship.Delete();

            foreach (var relationship in deletedRelationships)
                relationship.ChangeState(EntityState.Unchanged);
        }

        private bool RelationshipContainsKeyEntry(ObjectStateEntry stateEntry)
        {
            //prevent exception: "Cannot change state of a relationship if one of the ends of the relationship is a KeyEntry"
            //I haven't been able to find the conditions under which this happens, but it sometimes does.
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;
            var keys = new[] { stateEntry.OriginalValues[0], stateEntry.OriginalValues[1] };
            return keys.Any(key => objectContext.ObjectStateManager.GetObjectStateEntry(key).Entity == null);
        }

        internal async Task<IEnumerable<Payment>> GetAllPaymentsAsync(DateTime startDateCash, DateTime endDateCash)
        {
            try
            {
                endDateCash = endDateCash.AddDays(1);
                return await Context.Set<Payment>()
                    .Where(p => p.Date >= startDateCash && p.Date < endDateCash && p.Date >= Limit)
                        .Include(p => p.Customer)
                        .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<List<Expense>> GetAllExpensesAsync(Expression<Func<Expense, bool>> filterp)
        {
            return await Context.Expenses.Where(e => e.Date >= Limit).Where(filterp)
                .Include(e => e.User)
                .ToListAsync();
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
                await Context.Set<ShowUp>().Where(p2 => p2.Arrived >= Limit).ToListAsync();
                await Context.Set<Change>().Where(p3 => p3.Date >= CloseLimit).ToListAsync();
                await Context.Set<Apointment>().Where(p4 => p4.DateTime >= Limit).ToListAsync();
                await Context.Set<Program>().Where(p => p.StartDay >= Limit).ToListAsync();
                await Context.Set<Payment>().Where(p1 => p1.Date >= Limit).ToListAsync();

                var x = (await Context.Set<Customer>().Where(c => c.Enabled)
                        //.Include(o => o.Programs.Select(t => t.Payments))
                        .Select(c => new
                        {
                            c,
                            //Programs = c.Programs.Where(p => p.StartDay >= Limit),
                           //Payments = c.Payments.Where(p1 => p1.Date >= Limit),
                            c.WeightHistory,
                            c.Illness,
                             //ShowUps = c.ShowUps.Where(p2 => p2.Arrived >= Limit),
                              //Changes = c.Changes.Where(p3 => p3.Date >= CloseLimit),
                             //Apointments = c.Apointments.Where(p4 => p4.DateTime >= Limit)
                        })
                        .ToListAsync())
                    .Select(x1 => x1.c);

                return x;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public async Task<IEnumerable<Customer>> LoadAllCustomersAsyncb()
        //{
        //    try
        //    {
        //        var x = await Context.Set<Customer>()
        //            .Where(c => c.Enabled)
        //            .Include(z => z.Illness)
        //            .Include(z => z.ShowUps)
        //            .ToListAsync();
        //        return x.OrderByDescending(c => c.ActiveCustomer).ThenBy(g => g.SureName);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}