using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SAR.DAO.Base
{
    public static class ReflectionQueryable
    {
        private static readonly MethodInfo OrderByMethod =
            typeof(Queryable).GetMethods()
                .Where(method => method.Name == "OrderBy")
                .Where(method => method.GetParameters().Length == 2)
                .Single();

        private static readonly MethodInfo OrderByMethodDescending =
            typeof(Queryable).GetMethods()
                .Where(method => method.Name == "OrderByDescending")
                .Where(method => method.GetParameters().Length == 2)
                .Single();

        public static IQueryable<TSource> OrderByProperty<TSource>
            (this IQueryable<TSource> source, string propertyName, string direction)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "posting");
            Expression orderByProperty = Expression.Property(parameter, (propertyName ?? "MODIFY_TIME"));
            if ("DESC" == direction)
            {
                LambdaExpression lambda = Expression.Lambda(orderByProperty, new[] { parameter });
                MethodInfo genericMethod = OrderByMethodDescending.MakeGenericMethod
                    (new[] { typeof(TSource), orderByProperty.Type });
                object ret = genericMethod.Invoke(null, new object[] { source, lambda });
                return (IQueryable<TSource>)ret;
            }
            else
            {
                LambdaExpression lambda = Expression.Lambda(orderByProperty, new[] { parameter });
                MethodInfo genericMethod = OrderByMethod.MakeGenericMethod
                    (new[] { typeof(TSource), orderByProperty.Type });
                object ret = genericMethod.Invoke(null, new object[] { source, lambda });
                return (IQueryable<TSource>)ret;
            }
        }
    }
}
