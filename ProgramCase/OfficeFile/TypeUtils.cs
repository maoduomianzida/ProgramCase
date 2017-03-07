using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
#if INTERNALUTILIS
    internal static class TypeUtils
#else
    public static class TypeUtils
#endif 
    {
        public static object Convert(object value, Type convertType)
        {
            if (value == null)
            {
                return null;
            }
            if (convertType.IsInstanceOfType(value))
            {
                return value;
            }
            var typeConverter1 = TypeDescriptor.GetConverter(convertType);
            if (typeConverter1.CanConvertFrom(value.GetType()))
            {
                return typeConverter1.ConvertFrom(value);
            }
            var typeConverter2 = TypeDescriptor.GetConverter(value.GetType());
            if (typeConverter2.CanConvertTo(convertType))
            {
                return typeConverter2.ConvertTo(value, convertType);
            }

            return System.Convert.ChangeType(value, convertType);
        }

        public static object Create(Type type,params object[] paramArr)
        {
            return Activator.CreateInstance(type, paramArr);
        }

        /// <summary>
        /// 尝试创建某个类型的实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public static bool TryCreate(Type type, out object instance, params object[] paramArr)
        {
            instance = null;
            if (type != null)
            {
                try
                {
                    instance = Activator.CreateInstance(type, paramArr);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return true;
        }

        /// <summary>Convert the given object to a given strong type.
        /// </summary>
        public static T Convert<T>(object value)
        {
            object result = Convert(value, typeof(T));
            if (result == null)
            {
                return default(T);
            }
            else
            {
                return (T)result;
            }
        }

        public static bool TryConvert<T>(object value,out T realVal)
        {
            bool result = true;
            realVal = default(T);
            try
            {
                realVal = Convert<T>(value);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 是否继承接口
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool IsImplementInterface(Type instanceType, Type interfaceType)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException(nameof(instanceType));
            }
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            Type[] typeArr = instanceType.GetInterfaces();

            return typeArr.Any(type => ImplementInterface(type, interfaceType));
        }

        public static bool IsImplementInterface(Type instanceType, Type interfaceType, out Type implementType)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException(nameof(instanceType));
            }
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            Type[] typeArr = instanceType.GetInterfaces();
            implementType = typeArr.FirstOrDefault(type => ImplementInterface(type, interfaceType));

            return implementType != null;
        }

        private static bool ImplementInterface(Type type, Type interfaceType)
        {
            return type.IsGenericType ? interfaceType.IsAssignableFrom(type.GetGenericTypeDefinition()) : interfaceType.IsAssignableFrom(type);
        }
    }
}
