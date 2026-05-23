using System.Linq.Expressions;

namespace VoguMap.Application.Helpers
{
    public static class PredicateCombiner
    {
        public static Expression<Func<T, bool>> Combine<T>(
            Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T));
            var combined = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter));
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }
}