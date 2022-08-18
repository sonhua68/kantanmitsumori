using KantanMitsumori.Entity.ASESTEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace KantanMitsumori.Infrastructure.Base
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        protected ASESTContext _context;
        internal DbSet<TEntity> dbSet;
        public readonly ILogger _logger;

        public GenericRepository(ASESTContext context, ILogger logger)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
            _logger = logger;
        }

        public virtual bool Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual bool Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy)
        {
            if (orderBy == null)
            {
                return dbSet.ToList();
            }
            else
            {
                return orderBy(dbSet).ToList();
            }
        }

        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy)
        {
            IQueryable<TEntity> query = dbSet;
            query = query.Where(expression);
            if (orderBy != null)
            {
                orderBy(query);
            }
            return query.ToList();
        }

        public virtual TEntity? GetSingle(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy)
        {
            IQueryable<TEntity> query = dbSet;
            query = query.Where(predicate);
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> GetSkipAndTake(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, int skipRecord, int takeRecord)
        {
            IQueryable<TEntity> query = dbSet;
            query = query.Where(expression);
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Skip(skipRecord).Take(takeRecord).ToList();
        }

        public virtual TResult? Max<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            try
            {
                return dbSet.Max(selector);
            }
            catch
            {
                // No records in data set
                return default;
            }
        }

        public TResult? Min<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            try
            {
                return dbSet.Min(selector);
            }
            catch
            {
                // No records in data set
                return default;
            }
        }

        public virtual decimal Sum<TResult>(Expression<Func<TEntity, decimal>> selector)
        {
            try
            {
                return dbSet.Sum(selector);
            }
            catch
            {
                // No records in data set
                return 0;
            }
        }

        public virtual int Count<TResult>(Expression<Func<TEntity, bool>> selector)
        {
            try
            {
                return dbSet.Count(selector);
            }
            catch
            {
                // No records in data set
                return 0;
            }

        }


        public virtual IEnumerable<TEntity> FromSql(string queryString, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return dbSet.FromSqlRaw(queryString).ToList();
            }
            else
            {
                return dbSet.FromSqlRaw(queryString).AsNoTracking().ToList();
            }
        }


        public virtual TEntity? SingleFromSql(string queryString, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return dbSet.FromSqlRaw(queryString).FirstOrDefault();
            }
            else
            {
                return dbSet.FromSqlRaw(queryString).AsNoTracking().FirstOrDefault();
            }
        }

    }
}
