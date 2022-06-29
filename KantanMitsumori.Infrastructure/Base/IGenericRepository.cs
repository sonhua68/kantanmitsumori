using System.Linq.Expressions;

namespace KantanMitsumori.Infrastructure.Base
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        bool Add(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        TEntity? GetSingle(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        IEnumerable<TEntity> GetSkipAndTake(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int skipRecord = 0, int takeRecord = 0);
        TResult? Max<TResult>(Expression<Func<TEntity, TResult>> selector);
        TResult? Min<TResult>(Expression<Func<TEntity, TResult>> selector);
        int Count<TResult>(Expression<Func<TEntity, bool>> selector);
        decimal Sum<TResult>(Expression<Func<TEntity, decimal>> selector);

        IEnumerable<TEntity> FromSql(String queryString, bool allowTracking = true);

        TEntity? SingleFromSql(String queryString, bool allowTracking = true);
    }
}
