using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.ComponentModel;

namespace ProgramCase
{
    /// <summary>
    /// 两个类似类之间互相转换
    /// </summary>
    internal static class AutoMapHelper
    {
        private static ConcurrentDictionary<Type, Dictionary<string, PropertyHelper>> _propertyCache = new ConcurrentDictionary<Type, Dictionary<string, PropertyHelper>>();

        public static object Map(object source,Type targetType)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (targetType.IsInterface || targetType.IsAbstract)
            {
                throw new Exception("转换的类型不能是接口，抽象类");
            }
            Type sourceType = source.GetType();
            Dictionary<string, PropertyHelper> targetProArr = PropertyHelper.GetPropertys(targetType);
            Dictionary<string, PropertyHelper> sourceProArr = PropertyHelper.GetPropertys(sourceType);
            object instance = Activator.CreateInstance(targetType);
            foreach (var item in targetProArr)
            {
                PropertyHelper sourceHelper;
                if (sourceProArr.TryGetValue(item.Key, out sourceHelper))
                {
                    item.Value.SetValue(instance, TypeUtils.Convert(sourceHelper.GetValue(source), item.Value.Property.PropertyType));
                }
            }

            return instance;
        }

        public static T Map<T>(object source) where T : class
        {
            return Map(source,typeof(T)) as T;
        }
    }
}