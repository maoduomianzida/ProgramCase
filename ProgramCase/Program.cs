using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Type[] type = typeof(Program).Assembly.GetTypes();
            Type[] suitableTypes = FindSuitableTypes(type).ToArray();
            switch(suitableTypes.Length)
            {
                case 0: throw new Exception("未设置启动类！");
                case 1: Type mainType = suitableTypes.FirstOrDefault();
                    ICase main = Activator.CreateInstance(mainType) as ICase;
                    main.Run();
                    break;
                default:Console.WriteLine("设置了多个启动类");
                    foreach(Type tmp in suitableTypes)
                    {
                        Console.WriteLine(tmp.Name);
                    }
                    break;
            }
            Console.ReadKey();
        }

        static IEnumerable<Type> FindSuitableTypes(Type[] typeSources)
        {
            return from Type type in typeSources where FilterCondition(type) select type; 
        }

        static bool FilterCondition(Type type)
        {
            MainAttribute main = type.GetCustomAttributes(typeof(MainAttribute), true).Cast<MainAttribute>().FirstOrDefault();

            return typeof(ICase).IsAssignableFrom(type) && !type.IsAbstract && main != null;
        }
    }
}