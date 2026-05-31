using System.Linq.Expressions;

namespace Naseejk.core.Specifications
{
    public interface ISpecification<T>
    {
        // The Criteria property is the condition that will be used to filter the data from the database.
        Expression<Func<T, bool>> Criteria { get; }

        // this part is responsible for joins on another table , for example if you want to get a user with his roles, you will need to join the user table with the roles table, so you will need to add the roles table to the includes list.
        List<Expression<Func<T, object>>> Includes { get; }
    }
}