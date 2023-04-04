using System.Linq.Expressions;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = true)
    {
        return trackChanges
            ? RepositoryContext.Set<T>()
                .Where(expression)
            : RepositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking();
    }

    public void Create(T entity)
    {
        RepositoryContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        RepositoryContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        RepositoryContext.Set<T>().Remove(entity);
    }

    protected RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }
}