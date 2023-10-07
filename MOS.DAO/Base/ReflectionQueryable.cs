using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MOS.DAO.Base
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

        private static readonly MethodInfo ThenByMethod =
            typeof(Queryable).GetMethods()
                .Where(method => method.Name == "ThenBy")
                .Where(method => method.GetParameters().Length == 2)
                .Single();

        private static readonly MethodInfo ThenByMethodDescending =
            typeof(Queryable).GetMethods()
                .Where(method => method.Name == "ThenByDescending")
                .Where(method => method.GetParameters().Length == 2)
                .Single();

        public static IQueryable<TSource> OrderByProperty<TSource>
            (this IQueryable<TSource> source, string propertyName, string direction)
        {
            return OrderByProperty(source, propertyName, direction, false);
        }

        public static IQueryable<TSource> ThenByProperty<TSource>
            (this IQueryable<TSource> source, string propertyName, string direction)
        {
            return OrderByProperty(source, propertyName, direction, true);
        }

        private static IQueryable<TSource> OrderByProperty<TSource>
            (this IQueryable<TSource> source, string propertyName, string direction, bool isThen)
        {
            if (!string.IsNullOrWhiteSpace(propertyName) && !string.IsNullOrWhiteSpace(direction))
            {
                propertyName = OrderProcessor.GetOrderField<TSource>(propertyName);
                direction = OrderProcessor.GetOrderDirection(direction);

                ParameterExpression parameter = Expression.Parameter(typeof(TSource), "posting");
                Expression orderByProperty = Expression.Property(parameter, propertyName);
                if ("DESC" == direction)
                {
                    LambdaExpression lambda = Expression.Lambda(orderByProperty, new[] { parameter });
                    MethodInfo method = isThen ? ThenByMethodDescending : OrderByMethodDescending;
                    MethodInfo genericMethod = method.MakeGenericMethod
                        (new[] { typeof(TSource), orderByProperty.Type });
                    object ret = genericMethod.Invoke(null, new object[] { source, lambda });
                    return (IQueryable<TSource>)ret;
                }
                else
                {
                    LambdaExpression lambda = Expression.Lambda(orderByProperty, new[] { parameter });
                    MethodInfo method = isThen ? ThenByMethod : OrderByMethod;
                    MethodInfo genericMethod = method.MakeGenericMethod
                        (new[] { typeof(TSource), orderByProperty.Type });
                    object ret = genericMethod.Invoke(null, new object[] { source, lambda });
                    return (IQueryable<TSource>)ret;
                }
            }
            else
            {
                return source;
            }
        }
    }
}
