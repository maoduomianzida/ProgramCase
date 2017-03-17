using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.IO;

namespace ProgramCase.AutoFac
{
    //[Main]
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
            List<string> list = new List<string>();
            for(int i = 0;i < 3000 ;i++)
            {
                int t = (i + 1) % 3;
                switch(t)
                {
                    case 0: list.Add("content" + (i + 1)); break;
                    case 1: list.Add("content" + (i + 1)); break;
                    case 2: list.Add("content" + (i + 1)); break;
                }
            }

            List<List<string>> batchArr = new List<List<string>>();

            int count = list.Count;
            int batchCount = 1000;
            int times = count / batchCount;
            if (count % batchCount != 0)
            {
                times++;
            }
            for (int i = 0; i < times; i++)
            {
                int startIndex = i * batchCount;
                if (i == times - 1)
                {
                    int residueCount = list.Count - startIndex;
                    batchArr.Add(list.GetRange(startIndex, residueCount));
                }
                else
                {
                    batchArr.Add(list.GetRange(startIndex, batchCount));
                }
            }

            Console.ReadKey();
        }
    }
}