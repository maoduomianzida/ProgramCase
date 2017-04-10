using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.AutoMapper
{
    //[Main]
    public class AutoMapperCase : ICase
    {
        public void Run()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.ConstructServicesUsing(type =>
                {
                    if(typeof(IJsonFormatter).IsAssignableFrom(type))
                    {
                        string tyy = "123";
                    }

                    return Activator.CreateInstance(type);
                });
                cfg.AddProfile<TProfile>();
            });
            string a = "10";

            /*TempFormatter json = Mapper.Instance.Map<TempFormatter>(a,opts =>
            {
                opts.Items.Add("t",new TempFormatter());
            });*/

            Temp t = new Temp { A = 1, Value = "123" };
            CTemp tmp = Mapper.Instance.Map<CTemp>(t,ttt => ttt.AfterMap((p1,p2) =>
            {
                string d = "sdf";
            }));

            string ta = "123";
        }
    }

    public class Temp
    {
        public int A { get; set; }

        public string Value { get; set; }
    }

    public class  CTemp
    {
        public int A { get; set; }

        public TempFormatter Value { get; set; }
    }

    public class TempFormatter : IJsonFormatter
    {
        public object _value;

        public void Deserialize(string data)
        {
            _value = data;
        }

        public string Serialize()
        {
            return _value.ToString();
        }
    }



    public class SuperB : Collection<B>
    {
        public bool IsLeaf { get; set; }
    }

    public class TProfile : Profile
    {
        public TProfile()
        {
            CreateMap<DateTime, long>().ConvertUsing<DateTimeToLongConverter>();
            CreateMap<long, DateTime>().ConvertUsing<LongToDateTimeConverter>();
            CreateMap<string, IJsonFormatter>().ConvertUsing<StringJsonFormatterConverter>();
            CreateMap<IJsonFormatter, string>().ConvertUsing<JsonFormatterStringConverter>();
            CreateMap<Temp, CTemp>();
        }
    }

    public class A
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class B
    {
        public int Id { get; set; }

        public long CreateTime { get; set; }
    }
}
