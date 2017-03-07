using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
#if INTERNALUTILIS
    internal static class ExpressionCreater
#else
    public static class ExpressionCreater
#endif
    {
        private static ConcurrentDictionary<Type, Func<object, int, object>> _getIndexCache = new ConcurrentDictionary<Type, Func<object, int, object>>();
        private static ConcurrentDictionary<Type, Func<object, int>> _getLenthCache = new ConcurrentDictionary<Type, Func<object, int>>();
        private static ConcurrentDictionary<Type, Action<object, object>> _addEleCache = new ConcurrentDictionary<Type, Action<object, object>>();

        public static Func<object,int> GetListLengthFunc(Type listType)
        {
            return _getLenthCache.GetOrAdd(listType, ExpressionCreater.CreateGetListLengthFunc);
        }

        public static Func<object, int, object> GetIndexElementFunc(Type listType)
        {
            return _getIndexCache.GetOrAdd(listType, ExpressionCreater.CreateGetIndexElementFunc);
        }

        public static Action<object, object> GetAddElementFunc(Type listType)
        {
            return _addEleCache.GetOrAdd(listType, CreateAddElementFunc);
        }

        public static Func<object, int> CreateGetListLengthFunc(Type listType)
        {
            ParameterExpression listExp = Expression.Parameter(typeof(object));
            Expression castExp = Expression.Convert(listExp, listType);
            PropertyInfo propertyInfo = listType.GetProperty("Count");
            Expression getExp = Expression.Call(castExp, propertyInfo.GetMethod);
            Func<object, int> func = Expression.Lambda<Func<object, int>>(getExp, listExp).Compile();

            return func;
        }

        public static Func<object, int, object> CreateGetIndexElementFunc(Type listType)
        {
            ParameterExpression listExp = Expression.Parameter(typeof(object));
            ParameterExpression indexExp = Expression.Parameter(typeof(int));
            Expression castExp = Expression.Convert(listExp, listType);
            PropertyInfo propertyInfo = listType.GetProperty("Item");
            Expression getExp = Expression.Property(castExp, propertyInfo, indexExp);
            Func<object, int, object> func = Expression.Lambda<Func<object, int, object>>(getExp, listExp, indexExp).Compile();

            return func;
        }

        /// <summary>
        /// 创建集合添加元素的委托
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        public static Action<object, object> CreateAddElementFunc(Type listType)
        {
            Type elementType = null;
            if (listType.IsGenericType)
            {
                elementType = listType.GetGenericArguments()[0];
            }
            else
            {
                throw new Exception("无法获取到集合内元素的类型");
            }
            ParameterExpression listExp = Expression.Parameter(typeof(object));
            ParameterExpression eleExp = Expression.Parameter(typeof(object));
            Expression castExp = Expression.Convert(listExp, listType);
            Expression castEleExp = Expression.Convert(eleExp, elementType);
            MethodInfo addMethod = listType.GetMethod("Add");
            MethodCallExpression callExp = Expression.Call(castExp, addMethod, castEleExp);
            Action<object, object> action = Expression.Lambda<Action<object, object>>(callExp, listExp, eleExp).Compile();

            return action;
        }
    }
}
