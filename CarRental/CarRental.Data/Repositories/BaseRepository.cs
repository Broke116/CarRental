using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;

namespace CarRental.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private CarRentalDBEntities dbContext;
        private DbSet<T> _dbSet;

        #region declarations
        protected IDbFactory DbFactory { get; private set; }

        protected CarRentalDBEntities DbContext
        {
            get { return dbContext ?? (dbContext = DbFactory.Init()); }
        }

        public BaseRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<T>();
        }
        #endregion

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public virtual IQueryable<T> All
        {
            get
            {
                return GetAll();
            }
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] properties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var property in properties)
            {
                query = query.Include(property);
            }
            return query;
        }

        public virtual T GetSingle(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)// for concurrency
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
    }
}
