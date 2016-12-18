using System;
using System.Linq;
using System.Linq.Expressions;

namespace FreeBird.Infrastructure.Linq
{
    /// <summary>
    /// 查询表达式建造者。
    /// </summary>
    public static class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another, bool condition)
        {
            return condition ? one.And(another) : one;
        }

        public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another, bool condition)
        {
            return condition ? one.Or(another) : one;
        }

    }
}
