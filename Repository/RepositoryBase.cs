using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Repository
{
    // #TODO code smell and anti-pattern?!? https://stackoverflow.com/questions/71223648/asp-net-core-possible-null-reference-return-in-generic-repository
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext;

        // #TODO refactor candidate
        public RepositoryBase(RepositoryContext repositoryContext)
            => RepositoryContext = repositoryContext;


        public void Create(T entity) =>
            RepositoryContext.Set<T>().Add(entity);

        public void Delete(T entity) =>
            RepositoryContext.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges)
        {
            // disable tracking if you have no intention of modifying data
            var queryable = !trackChanges
                ? RepositoryContext.Set<T>().AsNoTracking()
                : RepositoryContext.Set<T>();

            return queryable;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            // disabling tracking improves read-only query performance
            var queryable = !trackChanges
                ? RepositoryContext.Set<T>().Where(expression).AsNoTracking()
                : RepositoryContext.Set<T>().Where(expression);

            return queryable;
        }

        public void Update(T entity) =>
            RepositoryContext.Set<T>().Update(entity);
    }
}
