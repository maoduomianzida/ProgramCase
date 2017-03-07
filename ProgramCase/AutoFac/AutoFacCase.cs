using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace ProgramCase.AutoFac
{
    [Main]
    public class AutoFacCase : ICase
    {
        private ContainerBuilder _builder;
        private IContainer _container;

        public void Init()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterAssemblyModules(typeof(Program).Assembly);
            _container = _builder.Build();
        }

        public void Run()
        {
            Init();
            IEnumerable<object> objectArr = _container.Resolve(typeof(IEnumerable<ICase>)) as IEnumerable<object>;
            IEnumerable<ICase> printArr = objectArr.Cast<ICase>();
            //IEnumerable<ICase> printArr = _container.Resolve<IEnumerable<ICase>>();
            foreach (ICase tmp in printArr)
            {
                Console.WriteLine(tmp.GetType().Name);
            }
        }
    }
}