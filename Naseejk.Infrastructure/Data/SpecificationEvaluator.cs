using Microsoft.EntityFrameworkCore;
using Naseejk.core.Entities;
using Naseejk.core.Specifications;
using Naseejk.core.Interfaces;

namespace Naseejk.Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity :class ,IBaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}