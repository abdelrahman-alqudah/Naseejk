using System.Linq.Expressions;

namespace Naseejk.core.Specifications
{
    // This class serves as a base for creating specifications that define criteria and includes for querying entities of type T.
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        // This Constructor initializes a new instance of the BaseSpecification class with the specified criteria expression.
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        // The Criteria property holds the expression that defines the filtering criteria for the specification.
        public Expression<Func<T, bool>> Criteria { get; }
        // The Includes property holds a list of expressions that define related entities to include in the query results.
        public List<Expression<Func<T, object>>> Includes { get; } = new();

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}