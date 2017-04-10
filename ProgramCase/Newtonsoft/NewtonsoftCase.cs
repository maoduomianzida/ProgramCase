using Newtonsoft.Json;
using Sodao.Juketool.Share.Customer;
using Sodao.JukeTool.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ProgramCase.Newtonsoft
{
    [Main]
    public class NewtonsoftCase : ICase
    {
        public void Init()
        {

        }

        public void Run()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<A, B>().ForMember(tmp => tmp.Str, opts => opts.Ignore())
                                                          .ForMember(tmp => tmp.Ids,opts => opts.ResolveUsing(c =>
                                                          {
                                                              List<long> l = new List<long>();
                                                              if (!string.IsNullOrWhiteSpace(c.Str))
                                                              {
                                                                  foreach (string tmp in c.Str.Split(','))
                                                                  {
                                                                      long value;
                                                                      if (long.TryParse(tmp, out value))
                                                                          l.Add(value);
                                                                  }
                                                              }

                                                              return l;
                                                          }));
                config.CreateMap<KeyValuePair<A, int>, B>().ConvertUsing((KeyValuePair<A,int> source,B target, ResolutionContext context) =>
                {
                    B b = context.Mapper.Map<B>(source.Key);
                    b.Age = source.Value;

                    return b;
                });
            });
            Dictionary<A, int> dic = new Dictionary<A, int>();
            dic.Add(new A { Id = 1, UserName = "wg", Str = "10,3,6" },11);
            dic.Add(new A { Id = 2, UserName = "wh", Str = ",30," },12);
            dic.Add(new A { Id = 3, UserName = "TT", Str = null },14);
            List<B> list = Mapper.Instance.Map<List<B>>(dic);

            Console.WriteLine(list.Count);
        }
    }

    public class A
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Str { get; set; }
    }

    public class B
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public long Str { get; set; }

        public List<long> Ids { get; set; }
    }
}