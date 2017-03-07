using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    /// <summary>
    /// 操作类属性帮助类
    /// 通过生成委托来获取，设置类属性的值，比纯反射来实现快几十倍
    /// </summary>
#if INTERNALUTILIS
    internal class PropertyHelper
#else
    public class PropertyHelper : ICustomAttributeProvider
#endif
    {
        private static ConcurrentDictionary<Type, Dictionary<string, PropertyHelper>> _propertyCache = new ConcurrentDictionary<Type, Dictionary<string, PropertyHelper>>();
        public static Dictionary<string, PropertyHelper> GetPropertys(Type type, Func<Type, PropertyInfo[]> getter = null)
        {
            return _propertyCache.GetOrAdd(type, tmpType => GetPropertyHelpers(tmpType, getter ?? GetPropertys));
        }

        private static PropertyInfo[] GetPropertys(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public);
        }

        private static Dictionary<string, PropertyHelper> GetPropertyHelpers(Type type, Func<Type, PropertyInfo[]> getter)
        {
            PropertyInfo[] proArr = getter(type);

            return (from pro in proArr select new PropertyHelper(pro)).ToDictionary(item => item.Name, item => item);
        }

        private Func<object, object> _getFunc;
        private Action<object, object> _setFunc;
        public PropertyInfo Property { get; private set; }

        public string Name
        {
            get
            {
                return Property.Name;
            }
        }

        public PropertyHelper(PropertyInfo property)
        {
            Contract.Assert(property != null);
            Property = property;
            BuildFunc();
        }

        private void BuildFunc()
        {
            BuildGetFunc();
            BuildSetFunc();
        }

        private void BuildGetFunc()
        {
            if (Property.CanRead)
            {
                ParameterExpression objParamExp = Expression.Parameter(typeof(object));
                Expression callExp = null;
                if (IsStaticProperty(Property))
                {
                    callExp = Expression.Call(null, Property.GetMethod);
                }
                else
                {
                    Expression instanceExp = Expression.Convert(objParamExp, Property.ReflectedType);
                    callExp = Expression.Call(instanceExp, Property.GetGetMethod());
                }
                Expression convertExp = Expression.Convert(callExp, typeof(object));
                _getFunc = Expression.Lambda<Func<object, object>>(convertExp, objParamExp).Compile();
            }
        }

        private bool IsStaticProperty(PropertyInfo property)
        {
            Contract.Assert(property != null);
            MethodInfo methodInfo = null;
            if (property.CanRead)
            {
                methodInfo = property.GetMethod;
            }
            else if (property.CanWrite)
            {
                methodInfo = property.SetMethod;
            }
            if (methodInfo != null)
            {
                return methodInfo.IsStatic;
            }

            return false;
        }

        private void BuildSetFunc()
        {
            if (Property.CanWrite)
            {
                ParameterExpression objParamExp = Expression.Parameter(typeof(object), "that");
                ParameterExpression valueParamExp = Expression.Parameter(typeof(object), "value");
                Expression valueConvertParamExp = Expression.Convert(valueParamExp, Property.PropertyType);
                Expression callExp = null;
                if (IsStaticProperty(Property))
                {
                    callExp = Expression.Call(null, Property.SetMethod, valueConvertParamExp);
                }
                else
                {
                    Expression instanceExp = Expression.Convert(objParamExp, Property.ReflectedType);
                    callExp = Expression.Call(instanceExp, Property.SetMethod, valueConvertParamExp);
                }
                _setFunc = Expression.Lambda<Action<object, object>>(callExp, objParamExp, valueParamExp).Compile();
            }
        }

        public void SetValue(object instance, object value)
        {
            _setFunc?.Invoke(instance, value);
        }

        public object GetValue(object instance)
        {
            return _getFunc?.Invoke(instance);
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return Property.GetCustomAttributes(attributeType, inherit);
        }

        public T[] GetCustomAttributes<T>(bool inherit)
        {
            object[] arr = GetCustomAttributes(typeof(T),inherit);

            return arr.OfType<T>().ToArray();
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return Property.GetCustomAttributes(inherit);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return Property.IsDefined(attributeType,inherit);
        }
    }
}