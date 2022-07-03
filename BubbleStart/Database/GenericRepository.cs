using BubbleStart.Helpers;
using BubbleStart.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;

namespace BubbleStart.Database
{
    public class GenericRepository : IDisposable
    {
        public MainDatabase Context;
        public DateTime Limit;
        public DateTime ThreeMonths;
        public readonly DateTime CloseLimit;

        public GenericRepository()
        {
            Context = new MainDatabase();

            if (DateTime.Today.Month > 7 && DateTime.Today.Day >= 20)
            {
                Limit = new DateTime(DateTime.Today.Year - 1, 8, 20);
            }
            else
            {
                Limit = new DateTime(DateTime.Today.Year - 2, 8, 20);
            }
            Limit = new DateTime();
            ThreeMonths = DateTime.Today.AddMonths(-3);

            CloseLimit = (DateTime.Today - Limit).TotalDays > 20 ? DateTime.Today.AddDays(-20) : Limit;
            Context.Database.Log = Console.Write;
        }

        internal bool HasChanges<TEntity>(TEntity entity) where TEntity : BaseModel
        {
            if (entity is WorkingRule wr)
            {
                if (Context.Entry(entity).State != EntityState.Unchanged)
                    return true;
                else if (wr.DailyWorkingShifts.Any(t => Context.Entry(t).State != EntityState.Unchanged))
                    return true;
            }
            return false;
        }

        internal void RollBack<TEntity>(TEntity entity) where TEntity : BaseModel
        {
            Context.Entry(entity).State = EntityState.Unchanged;
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

        internal async Task<List<ShowUp>> GetAllShowUpsInRangeAsyncsAsync(DateTime StartDate, DateTime EndDate, int CId = -1, bool nolimit = false)
        {
            return await Context.ShowUps
           .Where(s => (s.Arrived >= StartDate && s.Arrived < EndDate && (nolimit || s.Arrived >= Limit)) && (CId == -1 || CId == s.Customer.Id))
           .OrderBy(s => s.Arrived)
           .ToListAsync();
        }

        internal async Task<List<ShowUp>> GetAllShowUpsAsync(ProgramMode pMode)
        {
            return await Context.ShowUps
           .Where(s => s.ProgramModeNew == pMode)
           .Include(s => s.Prog)
           .OrderBy(s => s.Arrived)
           .ToListAsync();
        }

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
                .Include(a => a.Customer.ShowUps)
                .ToListAsync();
        }

        public async Task<List<Program>> GetProgramsFullAsync(Expression<Func<Program, bool>> filter)
        {
            return await Context.Programs
                .Where(filter)
                .Include(a => a.ShowUpsList)
                .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : BaseModel
        {
            if (filter == null)
            {
                return await Context.Set<TEntity>().ToListAsync();
            }

            return await Context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public Customer GetFullCustomerById(int id)
        {
            try
            {
                return Context.Set<Customer>()
                    .Where(c => c.Id == id)
                    .Include(f => f.Programs.Select(t => t.Payments))
                    .Include(g => g.Payments)
                    .Include(d => d.WeightHistory)
                    .Include(c => c.Illness)
                    .Include(e => e.ShowUps)
                    .Include(h => h.Changes)
                    .Include(a => a.Apointments)
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Customer> GetFullCustomerByIdAsync(int id)
        {
            try
            {
                var thisWeek = StaticResources.GetNextWeekday(DateTime.Today, DayOfWeek.Monday).AddDays(-7);

                await Context.Set<ShowUp>()
                    .Where(s => s.Customer.Id == id && s.Arrived >= s.Customer.ResetDate || s.Arrived >= thisWeek)
                    .ToListAsync();

                await Context.Set<Change>()
                    .Where(s => s.Customer.Id == id && s.Date >= s.Customer.ResetDate)
                    .ToListAsync();
                await Context.Set<Apointment>()
                    .Where(p4 => p4.Customer.Id == id && p4.DateTime >= CloseLimit)
                    .ToListAsync();
                await Context.Set<Program>()
                    .Where(p => p.Customer.Id == id && p.StartDay >= p.Customer.ResetDate)
                    .ToListAsync();
                await Context.Set<Payment>()
                    .Where(s => s.Customer.Id == id && s.Program.StartDay >= s.Customer.ResetDate || (s.Program == null && s.Date >= s.Customer.ResetDate))
                    .ToListAsync();

                return await Context.Set<Customer>()
                    .Where(c => c.Id == id)
                    .Include(d => d.WeightHistory)
                    .Include(c => c.Illness)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<List<WorkingRule>> GetAllRulesAsync()
        {
            return await Context.WorkingRules
                .Where(w => w.To >= DateTime.Today)
                .Include(t => t.DailyWorkingShifts)
                .ToListAsync();
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
            //IEnumerable<DbEntityEntry> res = from e in Context.ChangeTracker.Entries()
            //                                 where
            //                                 e.State.HasFlag(EntityState.Added) ||
            //                                 e.State.HasFlag(EntityState.Modified) ||
            //                                 e.State.HasFlag(EntityState.Deleted)
            //                                 select e;
            return Context.ChangeTracker.HasChanges();
            //return res.Any();
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
            //IEnumerable<DbEntityEntry> changes = from e in Context.ChangeTracker.Entries()
            //                                     where e.State != EntityState.Unchanged
            //                                     select e;

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


        public void Save()
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
                    E.Property("ModifiedDate").CurrentValue = DateTime.Now;
                }
            });
            Context.SaveChanges();
        }

        public bool RollBack()
        {
            try
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
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Η επαναφορά απέτυχε, παρακαλώ βγείτε και ξαναμπείτε");
                return false;
            }
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

        internal async Task<IEnumerable<Payment>> GetAllPaymentsAsync(DateTime startDateCash, DateTime endDateCash, bool limit = true)
        {
            try
            {
                endDateCash = endDateCash.AddDays(1);
                return await Context.Payments
                    .Where(p => p.Customer.Enabled && p.Date >= startDateCash && p.Date <= endDateCash && (!limit || p.Date >= Limit))
                        .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<List<Expense>> GetAllExpensesAsync(Expression<Func<Expense, bool>> filterp, bool limit = true, List<int> expensetypes = null,int mainId=-1,int secId=-1)
        {
            if (expensetypes == null || expensetypes.Count == 0)
                return await Context.Expenses.Where(e => (!limit || e.Date >= Limit) &&
                (mainId <= 0 || mainId == e.MainCategoryId) && 
                (secId <= 0 || secId == e.SecondaryCategoryId)).Where(filterp)
                   .ToListAsync();
            else
                return await Context.Expenses.Where(e => e.MainCategoryId != null && (!limit || e.Date >= Limit) &&
                (mainId <= 0 || mainId == e.MainCategoryId) &&
                (secId <= 0 || secId == e.SecondaryCategoryId) && expensetypes.Contains((int)e.MainCategoryId)).Where(filterp)
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

        internal async Task<List<ClosedHour>> GetAllClosedHoursAsync(RoomEnum room, DateTime time)
        {
            List<DateTime> dates = new List<DateTime>();
            var limit = time.AddMonths(3);
            while (time < limit)
            {
                dates.Add(time);
                time = time.AddDays(7);
            }

            return await Context.ClosedHours.Where(e => e.Room == room && dates.Any(d => d == e.Date)).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsyncSortedByUserName()
        {
            return await Context.Set<User>().OrderBy(x => x.UserName).ToListAsync();
        }

        public async Task<IEnumerable<Customer>> LoadAllCustomersAsync()
        {
            try
            {
                var thisWeek = StaticResources.GetNextWeekday(DateTime.Today, DayOfWeek.Monday).AddDays(-7);
                await Context.Set<ShowUp>()
                    .Where(s => s.Customer.Enabled && (s.Arrived >= s.Customer.ResetDate || s.Arrived >= thisWeek))
                    .ToListAsync();
                //await Context.Set<ShowUp>()
                //   .Where(s => s.Arrived >= ThreeMonths) 
                //   .ToListAsync();
                await Context.Set<Change>()
                    .Where(s => s.Customer.Enabled && s.Date >= s.Customer.ResetDate)
                    .ToListAsync();
                await Context.Set<Apointment>()
                    .Where(p4 => p4.Customer.Enabled && p4.DateTime >= CloseLimit)
                    .ToListAsync();
                await Context.Set<Program>()
                    .Where(p => p.Customer.Enabled && p.StartDay >= p.Customer.ResetDate)
                    .ToListAsync();
                await Context.Set<Payment>()
                    .Where(s => s.Customer.Enabled &&( s.Program.StartDay >= s.Customer.ResetDate || (s.Program == null && s.Date >= s.Customer.ResetDate)))
                    .ToListAsync();

                //await Context.Set<ShowUp>().Where(s => s.Arrived >= Limit).ToListAsync();
                //await Context.Set<Change>().Where(s => s.Date >= s.Customer.ResetDate).ToListAsync();
                //await Context.Set<Apointment>().Where(p4 => p4.DateTime >= CloseLimit).ToListAsync();
                //var pr = await Context.Set<Program>().Where(p => p.StartDay >= Limit).ToListAsync();
                //await Context.Set<Payment>().Where(s => s.Program.StartDay >= s.Customer.ResetDate || (s.Program == null && s.Date >= s.Customer.ResetDate)).ToListAsync();

                var x = (await Context.Set<Customer>().Where(c => c.Enabled)
                        .Select(c => new
                        {
                            c,
                            c.WeightHistory,
                            c.Illness,
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

        internal async Task<List<Apointment>> GetAllAppointmentsThisDayAsync(int id, DateTime time, RoomEnum room)
        {
            List<DateTime> dates = new List<DateTime>();
            var limit = time.AddMonths(3);
            while (time.Month != 8)
            {
                dates.Add(time);
                time = time.AddDays(7);
            }

            return await Context.Apointments.Where(a => a.Customer.Id == id && dates.Any(d => d == a.DateTime)).ToListAsync();
        }
    }
}