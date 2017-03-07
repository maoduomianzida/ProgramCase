using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    internal static class ExpressionParser
    {
        public static PropertyInfo ParserProperty<T>(Expression<Func<T,object>> exp)
        {
            Contract.Assert(exp != null);
            PropertyInfo result;
            if (exp.Body.NodeType == ExpressionType.Convert)
                result = ((MemberExpression)((UnaryExpression)exp.Body).Operand).Member as PropertyInfo;
            else result = ((MemberExpression)exp.Body).Member as PropertyInfo;

            if (result != null)
                return typeof(T).GetProperty(result.Name);

            throw new ArgumentException(string.Format("无法从表达式 '{0}' 中获取到属性.", exp.ToString()));
        }
    }
}
